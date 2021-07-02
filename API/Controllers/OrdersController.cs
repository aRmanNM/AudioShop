using System;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperService _mapper;

        public OrdersController(IOrderRepository orderRepository,
            ICouponRepository couponRepository,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IMapperService mapper)
        {
            _couponRepository = couponRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(BasketDto basketDto)
        {
            var user = await _userManager.FindByIdAsync(basketDto.UserId);

            // TODO: Create mapper
            var order = new Order
            {
                UserId = basketDto.UserId,
                TotalPrice = basketDto.TotalPrice,
                Status = false,
                Date = DateTime.Now,
                Discount = basketDto.Discount,
                PriceToPay = basketDto.PriceToPay,
                OtherCouponCode = basketDto.OtherCouponCode,
                SalespersonCouponCode = basketDto.SalespersonCouponCode
            };

            // TODO: maybe we should create order episode items when order is successfull
            order.OrderEpisodes = basketDto.EpisodeIds.Select(e => new OrderEpisode {
                OrderId = order.Id,
                EpisodeId = e
            }).ToArray();

            await _orderRepository.CreateOrderAsync(order);
            await _unitOfWork.CompleteAsync();

            await _userManager.UpdateAsync(user);

            return Ok(order);
        }

        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, true);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(_mapper.MapOrderToOrderWithUserInfo(order));
        }

        [HttpPut("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderWithUserInfo orderWithInfo)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, true);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = !order.Status;
            order.PaymentReceipt = orderWithInfo.PaymentReceipt;

            if (order.Status && !string.IsNullOrEmpty(order.OtherCouponCode))
            {
                var coupon = await _couponRepository.GetCouponByCodeAsync(order.OtherCouponCode);
                if (coupon != null)
                {
                    coupon.Blacklist.Add(new BlacklistItem()
                    {
                        CouponCode = order.OtherCouponCode,
                        UserId = order.UserId
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.MapOrderToOrderWithUserInfo(order));
        }
    }
}