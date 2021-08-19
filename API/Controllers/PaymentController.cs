using System;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
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

        public PaymentController(IOrderRepository orderRepository,
            IConfiguration config,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICouponRepository couponRepository,
            IZarinPalService zarinPalService,
            UserManager<User> userManager)
        {
            _config = config;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _couponRepository = couponRepository;
            _zarinPalService = zarinPalService;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        [HttpPost("payorder")]
        public async Task<IActionResult> PayOrder([FromBody] Order order)
        {
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
            }, PaymentMode.zarinpal); // toggle sandbox here

            return Redirect($"https://zarinpal.com/pg/StartPay/{result.Data.Authority}"); // also change here for sandbox

        }

        [HttpGet]
        [Route("PaymentResult/{orderId}")]
        public async Task<ActionResult<PaymentResultDto>> PaymentResult(int orderId)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
               HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
               HttpContext.Request.Query["Authority"] != "")
            {
                var authority = HttpContext.Request.Query["Authority"].ToString();
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                var verification = await _zarinPalService.Verification(new PaymentVerificationDto()
                {
                    Amount = (int)(order.PriceToPay),
                    MerchantId = _config["MerchantId"],
                    Authority = authority
                }, PaymentMode.zarinpal); // toggle sandbox here

                if (verification.Data.Code != 100)
                {
                    return BadRequest();
                }

                order.Status = true;
                order.PaymentReceipt = verification.Data.RefId.ToString();

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

                if (order.OrderType == OrderType.MonthlySub ||
                    order.OrderType == OrderType.HalfYearlySub ||
                    order.OrderType == OrderType.YearlySub)
                {
                    var user = await _userManager.FindByIdAsync(order.UserId);
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

                await _unitOfWork.CompleteAsync();
                return View(new PaymentResultDto
                {
                    PaymentSucceded = true,
                    RefId = verification.Data.RefId
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