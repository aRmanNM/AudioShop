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

            return Ok(order);
        }


    }
}