using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZarinpalSandbox;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;
        public PaymentController(IOrderService orderService, IConfiguration config)
        {
            _config = config;
            _orderService = orderService;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("PaymentVerification/{orderId}")]
        public async Task<IActionResult> PaymentVerification(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return View(order);
        }

        [HttpPost("payorder")]
        public IActionResult PayOrder(Order order)
        {
            if (order.Status)
            {
                return BadRequest();
            }

            var payment = new Payment(order.TotalPrice);
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
                var order = await _orderService.GetOrderById(orderId);
                var payment = new Payment(order.TotalPrice);
                var result = payment.Verification(authority).Result;

                if (result.Status != 100)
                {
                    return BadRequest();
                }
                order.Status = true;
                await _orderService.SaveChanges();
                return View(new PaymentResultDto
                {
                    RefId = result.RefId
                });

            }

            return BadRequest();
        }
    }
}