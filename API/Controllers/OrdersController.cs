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
            // CREATE LIST OF COUPONS TO APPLY!
            //var couponsToUse = new List<Coupon>();
            var coupon = await _couponRepository.GetCouponByCode(basketDto.CouponCode);
            //if (coupon.IsActive) { couponsToUse.Add(coupon); }
            var user = await _userManager.FindByIdAsync(basketDto.UserId);
            //if (user.Coupon != null && user.Coupon.IsActive == true) { couponsToUse.Add(user.Coupon); }
            //couponsToUse = couponsToUse.Distinct().ToList();

            //var totalPrice = basketDto.Episodes.Select(c => c.Price).Aggregate((sum, p) => sum + p);

            //decimal discount = 0;
            //decimal mem = totalPrice;
            //foreach (var c in couponsToUse)
            //{
            //    discount += (mem * c.DiscountPercentage) / 100;
            //    mem -= discount;
            //}

            var order = new Order
            {
                UserId = basketDto.UserId,
                TotalPrice = basketDto.TotalPrice,
                Status = false,
                Date = DateTime.Now,
                Discount = basketDto.Discount,
                PriceToPay = basketDto.PriceToPay,
                Episodes = basketDto.Episodes.Select(e => _mapper.MapEpisodeDtoToEpisode(e)).ToArray(),
            };

            order.Coupons.Add(coupon);
            await _orderRepository.CreateOrder(order);

            if (user.CouponId == null && coupon.UserId != null) { user.CouponId = coupon.Id; }
            await _userManager.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return Ok(order);
        }
    }
}