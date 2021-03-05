using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Dto.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZarinPal.Class;

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

        // zarinpal stuff
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;

        public PaymentController(IOrderRepository orderRepository,
            IConfiguration config,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICouponRepository couponRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _couponRepository = couponRepository;
            _orderRepository = orderRepository;

            // zarinpal stuff
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
        }

        [HttpPost("payorder")]
        public async Task<IActionResult> PayOrder([FromBody] Order order)
        {
            if (order.Status)
            {
                return BadRequest();
            }

            var result = await _payment.Request(new DtoRequest()
            {
                CallbackUrl = _config["ApiUrl"] + "api/Payment/PaymentResult/" + order.Id,
                Description = "توضیحات",
                Amount = (int)order.PriceToPay,
                MerchantId = _config["MerchantId"]
            }, Payment.Mode.sandbox); // TODO: CHANGE THIS FOR PRODUCTION
            return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");
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
                var order = await _orderRepository.GetOrderById(orderId);
                var verification = await _payment.Verification(new DtoVerification
                {
                    Amount = (int)order.PriceToPay,
                    MerchantId = _config["MerchantId"],
                    Authority = authority
                }, Payment.Mode.sandbox); // TODO: CHANGE THIS FOR PRODUCTION

                if (verification.Status != 100)
                {
                    return BadRequest();
                }

                order.Status = true;
                order.PaymentReceipt = verification.RefId.ToString();

                var salesperson = await _userRepository.GetSalespersonByCouponCode(order.SalespersonCouponCode);
                if (salesperson != null)
                {
                    var salespersonShare = (order.PriceToPay * salesperson.SalePercentageOfSalesperson) / 100;
                    order.SalespersonShare = salespersonShare;
                    salesperson.CurrentSalesOfSalesperson += salespersonShare;
                }

                var coupon = await _couponRepository.GetCouponByCode(order.OtherCouponCode);
                if (coupon != null)
                {
                    coupon.Blacklist.Add(new BlacklistItem()
                    {
                        CouponCode = order.OtherCouponCode,
                        UserId = order.UserId
                    });
                }

                await _unitOfWork.CompleteAsync();
                return View(new PaymentResultDto
                {
                    RefId = verification.RefId
                });
            }

            return BadRequest();
        }
    }
}