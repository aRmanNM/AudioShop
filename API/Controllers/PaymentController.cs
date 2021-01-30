using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZarinpalSandbox;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IOrderRepository orderRepository,
            IConfiguration config,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _config = config;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        [HttpPost("payorder")]
        public IActionResult PayOrder([FromBody] Order order)
        {
            if (order.Status)
            {
                return BadRequest();
            }

            var payment = new Payment((int)order.PriceToPay);
            var result = payment.PaymentRequest($"پرداخت فاکتور شماره {order.Id}",
                _config["ApiUrl"] + "api/Payment/PaymentResult/" + order.Id);

            if (result.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
            }
            else
            {
                return BadRequest();
            }
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
                var payment = new Payment((int)order.PriceToPay);
                var result = payment.Verification(authority).Result;

                if (result.Status != 100)
                {
                    return BadRequest();
                }

                order.Status = true;
                order.PaymentReceipt = result.RefId.ToString();

                var salesperson = await _userRepository.GetSalespersonByCouponCode(order.SalespersonCouponCode);

                var salespersonShare = order.PriceToPay - ((order.PriceToPay * salesperson.SalePercentage) / 100);
                order.SalespersonShare = salespersonShare;
                salesperson.TotalSales +=  salespersonShare;

                // TODO: ORDER UPDATE FAILURE - HOW TO HANDLE
                var res = await _unitOfWork.CompleteAsync();
                if (res < 0)
                {
                    return BadRequest("failed to update order");
                }

                return View(new PaymentResultDto
                {
                    RefId = result.RefId
                });

            }

            return BadRequest();
        }
    }
}