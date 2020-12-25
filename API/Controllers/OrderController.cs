using System;
using System.Linq;
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
    [ApiController]
    [Route("api/[controller]")]

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;
        public OrderController(IOrderService orderService, IConfiguration config)
        {
            _config = config;
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateOrder(BasketDto basketDto)
        {
            var order = new Order
            {
                UserId = basketDto.UserId,
                TotalPrice = basketDto.TotalPrice,
                Status = false,
                Date = DateTime.Now
            };

            await _orderService.CreateOrder(order);

            var basketItems = basketDto.CourseDtos?.Select(c => new BasketItem
            {
                OrderId = order.Id,
                CourseId = c.Id,
                Price = c.Price
            });

            await _orderService.CreateBasketItems(basketItems);

            return RedirectToAction(nameof(PayOrder), order);
        }

        [HttpPost("payorder")]
        [Authorize]
        public ActionResult PayOrder(Order order)
        {
            if (order.Status)
            {
                return BadRequest();
            }

            var payment = new Payment(order.TotalPrice);
            var result = payment.PaymentRequest($"پرداخت فاکتور شماره {order.Id}",
                _config["ApiUrl"] + "api/Order/PaymentResult/" + order.Id);

            if(result.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("PaymentResult/{orderId}")]
        public async Task<ActionResult<PaymentResultDto>> PaymentResult(int orderId)
        {
            if(HttpContext.Request.Query["Status"] != "" &&
               HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
               HttpContext.Request.Query["Authority"] != "")
            {
                var authority = HttpContext.Request.Query["Authority"].ToString();
                var order = await _orderService.GetOrderById(orderId);
                var payment = new Payment(order.TotalPrice);
                var result = payment.Verification(authority).Result;

                if(result.Status != 100)
                {
                    return BadRequest();
                }
                    order.Status = true;
                    await _orderService.SaveChanges();
                    return View(new PaymentResultDto{
                        RefId = result.RefId
                    });

            }

            return BadRequest();
        }
    }
}