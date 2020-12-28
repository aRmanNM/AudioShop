using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _config;
        public OrderController(IOrderRepository orderRepository, IConfiguration config)
        {
            _config = config;
            _orderRepository = orderRepository;
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

            await _orderRepository.CreateOrder(order);

            var basketItems = basketDto.CourseDtos?.Select(c => new BasketItem
            {
                OrderId = order.Id,
                CourseId = c.Id,
                Price = c.Price
            });

            await _orderRepository.CreateBasketItems(basketItems);

            return Ok(order);
        }


    }
}