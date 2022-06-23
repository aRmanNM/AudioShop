using System;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Models.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICouponRepository _couponRepository;
        private readonly IZarinPalService _zarinPalService;
        private readonly UserManager<User> _userManager;
        private readonly IConfigRepository _configRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ISMSService _smsService;

        public PaymentController(IOrderRepository orderRepository,
            IConfiguration config,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICouponRepository couponRepository,
            IZarinPalService zarinPalService,
            UserManager<User> userManager,
            IConfigRepository configRepository,
            IMessageRepository messageRepository,
            ISMSService smsService)
        {
            _config = config;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _couponRepository = couponRepository;
            _zarinPalService = zarinPalService;
            _userManager = userManager;
            _configRepository = configRepository;
            _messageRepository = messageRepository;
            _smsService = smsService;
            _orderRepository = orderRepository;
        }

        [HttpPost("payorder")]
        public async Task<IActionResult> PayOrder([FromQuery] int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order.Status)
            {
                return BadRequest();
            }

            string description = order.OrderType switch
            {
                OrderType.Episode => "خرید قسمت جدید",
                OrderType.Course => "خرید دوره جدید",
                OrderType.MonthlySub => "خرید اشتراک ماهیانه",
                OrderType.HalfYearlySub => "خرید اشتراک شش ماهه",
                OrderType.YearlySub => "خرید اشتراک سالیانه",
                _ => "خرید محصول جدید"
            };

            var result = await _zarinPalService.Request(new PaymentRequestDto()
            {
                CallbackUrl = _config["ApiUrl"] + "api/Payment/PaymentResult/" + order.Id,
                Description = description,
                Amount = (int)(order.PriceToPay),
                MerchantId = _config["MerchantId"]
            }, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? PaymentMode.sandbox : PaymentMode.zarinpal);

            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}")
                : Redirect($"https://zarinpal.com/pg/StartPay/{result.Data.Authority}");

        }

        [HttpGet]
        [Route("PaymentResult/{orderId}")]
        public async Task<ActionResult<PaymentResultDto>> PaymentResult(int orderId)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
               HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
               HttpContext.Request.Query["Authority"] != "")
            {

                var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Demo";

                var authority = HttpContext.Request.Query["Authority"].ToString();
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                var verification = await _zarinPalService.Verification(new PaymentVerificationDto()
                {
                    Amount = (int)(order.PriceToPay),
                    MerchantId = _config["MerchantId"],
                    Authority = authority
                }, isDev ? PaymentMode.sandbox : PaymentMode.zarinpal);

                if (!isDev && verification.Data.Code != 100)
                {
                    return BadRequest();
                }

                order.Status = true;
                order.PaymentReceipt = isDev ? "123456" : verification.Data.RefId.ToString();

                var salesperson = await _userRepository.GetSalespersonByCouponCodeAsync(order.SalespersonCouponCode);
                if (salesperson != null)
                {
                    var salespersonShare = (order.PriceToPay * salesperson.SalePercentageOfSalesperson) / 100;
                    order.SalespersonShare = salespersonShare;
                    salesperson.CurrentSalesOfSalesperson += salespersonShare;
                }

                var coupon = await _couponRepository.GetCouponByCodeAsync(order.OtherCouponCode);
                if (coupon != null)
                {
                    coupon.Blacklist.Add(new BlacklistItem()
                    {
                        CouponCode = order.OtherCouponCode,
                        UserId = order.UserId
                    });
                }

                var user = await _userManager.FindByIdAsync(order.UserId);
                if (order.OrderType == OrderType.MonthlySub ||
                    order.OrderType == OrderType.HalfYearlySub ||
                    order.OrderType == OrderType.YearlySub)
                {
                    switch (order.OrderType)
                    {
                        case OrderType.MonthlySub:
                            user.SubscriptionType = SubscriptionType.Monthly;
                            user.SubscriptionExpirationDate = DateTime.Today.AddMonths(1);
                            break;

                        case OrderType.HalfYearlySub:
                            user.SubscriptionType = SubscriptionType.HalfYearly;
                            user.SubscriptionExpirationDate = DateTime.Today.AddMonths(6);
                            break;

                        case OrderType.YearlySub:
                            user.SubscriptionType = SubscriptionType.Yearly;
                            user.SubscriptionExpirationDate = DateTime.Today.AddMonths(12);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    };

                    await _userManager.UpdateAsync(user);
                }

                // Send Message To User
                var generalConfigs = (await _configRepository.GetConfigsByGroupAsync("General")).ToList();
                string messageBody = "";
                if (order.OrderType == OrderType.Course || order.OrderType == OrderType.Episode)
                {
                    messageBody = generalConfigs.FirstOrDefault(gc => gc.TitleEn == "CoursePaidMessage").Value
                        + $"\nکد سفارش: {order.Id}";
                }
                else if (order.OrderType == OrderType.MonthlySub ||
                    order.OrderType == OrderType.HalfYearlySub ||
                    order.OrderType == OrderType.YearlySub)
                {
                    messageBody = generalConfigs.FirstOrDefault(gc => gc.TitleEn == "SubscriptionPaidMessage").Value
                        + $"\nکد سفارش: {order.Id}";
                }

                var message = new Message
                {
                    Title = "اعلان خرید",
                    Body = messageBody,
                    MessageType = MessageType.User,
                    IsRepeatable = false,
                    Link = "",
                    SendPush = true,
                    SendSMS = true,
                    SendInApp = true,
                    UserId = order.UserId,
                    CreatedAt = DateTime.Now,
                    CourseId = 0,
                };

                if (message.MessageType == MessageType.User)
                {
                    var userMessage = new UserMessage
                    {
                        MessageId = message.Id,
                        UserId = message.UserId,
                        PushSent = false,
                        SMSSent = false,
                        InAppSeen = false
                    };

                    message.UserMessages.Add(userMessage);

                    if (message.SendSMS)
                    {
                        if (user.PhoneNumberConfirmed)
                        {
                            _smsService.SendMessageSMS(user.PhoneNumber, message.Body);
                        }

                        userMessage.SMSSent = true;
                    }
                }

                await _messageRepository.CreateMessageAsync(message);
                await _unitOfWork.CompleteAsync();
                if (message.SendSMS && user.PhoneNumberConfirmed)
                {
                    _smsService.SendMessageSMS(user.PhoneNumber, message.Body);
                }

                return View(new PaymentResultDto
                {
                    PaymentSucceded = true,
                    RefId = isDev ? 123456 : verification.Data.RefId
                });
            }

            return View(new PaymentResultDto
            {
                PaymentSucceded = false,
                RefId = 0
            });
        }
    }
}