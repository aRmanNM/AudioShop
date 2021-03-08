using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CouponsController(ICouponRepository couponRepository,
            IMapperService mapper,
            IUnitOfWork unitOfWork)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetCoupons()
        {
            var coupons = await _couponRepository.GetCoupons();
            return Ok(coupons);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Coupon>> CreateCoupon(CouponToCreateDto couponDto)
        {
            var coupon = await _mapper.MapCouponDtoToCoupon(couponDto);
            await _couponRepository.CreateCoupon(coupon);
            await _unitOfWork.CompleteAsync();
            return Ok(coupon);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Coupon>> UpdateCoupon(Coupon coupon)
        {            
            _couponRepository.UpdateCoupon(coupon);
            await _unitOfWork.CompleteAsync();
            return Ok(coupon);
        }

        [HttpGet("{couponCode}/isActive")]
        public async Task<bool> CheckCouponIsActive(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode);
            if (coupon == null)
            {
                return false;
            }

            if (coupon.IsActive)
            {
                return true;
            }

            return false;
        }

        [HttpGet("{couponCode}/IsSalespersonCoupon")]
        public async Task<int> CheckIsSalespersonCoupon(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode);
            if (coupon == null)
            {
                return -1;
            }

            if (!string.IsNullOrEmpty(coupon.UserId) && coupon.IsActive == true)
            {
                return coupon.DiscountPercentage;
            }

            return -1;
        }
    }
}
