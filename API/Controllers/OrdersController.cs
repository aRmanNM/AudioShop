using System;
using System.Collections.Generic;
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
            var coupon = await _couponRepository.GetCouponByCode(basketDto.OtherCouponCode);
            var user = await _userManager.FindByIdAsync(basketDto.UserId);

            var order = new Order
            {
                UserId = basketDto.UserId,
                TotalPrice = basketDto.TotalPrice,
                Status = false,
                Date = DateTime.Now,
                Discount = basketDto.Discount,
                PriceToPay = basketDto.PriceToPay
            };

            // TODO: TEST THIS!
            if (coupon.UserId != null && user.CouponCode != null)
            {
                order.SalespersonCouponCode = coupon.Code;
            }
            else if (coupon.UserId != null && user.CouponCode == null)
            {
                order.SalespersonCouponCode = coupon.Code;
                user.CouponCode = coupon.Code;
            }
            else
            {
                order.OtherCouponCode = coupon.Code;
            }

            order.OrderEpisodes = basketDto.Episodes.Select(e => new OrderEpisode {
                OrderId = order.Id,
                EpisodeId = e.Id
            }).ToArray();

            await _orderRepository.CreateOrder(order);
            await _unitOfWork.CompleteAsync();

            await _userManager.UpdateAsync(user);

            return Ok(order);
        }
    }
}