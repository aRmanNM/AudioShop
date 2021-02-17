using System.Linq;
using System.Security.Claims;
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
    [Authorize(Roles="Member")]
    public class MemberController : ControllerBase
    {
        private readonly IMapperService _mapper;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly UserManager<User> _userManager;

        public MemberController(
            IMapperService mapper,
            IEpisodeRepository episodeRepository,
            ICouponRepository couponRepository,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _episodeRepository = episodeRepository;
            _couponRepository = couponRepository;
            _userManager = userManager;
        }

        [HttpGet("Episodes/{userId}")]
        public async Task<ActionResult<EpisodeDto>> GetUserEpisodes(string userId)
        {
            var episodes = await _episodeRepository.GetUserEpisodes(userId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e)).ToArray();
            return Ok(episodeDtos);
        }

        [HttpPost("refinebasket")]
        public async Task<ActionResult<BasketDto>> Refinebasket(BasketDto basketDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEpisodeIds = await _episodeRepository.GetUserEpisodeIds(userId);

            // TODO: Check this for validity
            var filteredEpisdeIds = basketDto.EpisodeIds.Where(epi => !userEpisodeIds.Any(uepi => uepi == epi)).ToArray();
            basketDto.EpisodeIds = filteredEpisdeIds;

            return basketDto;
        }

        [HttpGet("CheckIfEpisodeIsRepetitive/{userId}/{episodeId}")]
        public async Task<ActionResult<bool>> CheckIfCourseIsRepetitive(string userId, int episodeId)
        {
            var episodeIds = await _episodeRepository.GetUserEpisodeIds(userId);

            foreach(var tempEpisodeId in episodeIds)
            {
                if(tempEpisodeId == episodeId)
                {
                    return true;
                }
            }

            return false;
        }

        [HttpGet("canUseCoupon/{couponCode}")]
        public async Task<int> CheckMemeberCanUseCoupon(string couponCode)
        {
            // If coupon is inactive or null
            // no need to check if memeber is blacklisted or not

            // -1 means cannot

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var coupon = await _couponRepository.GetCouponByCode(couponCode);

            if (coupon == null)
            {
                return -1;
            }

            if (!coupon.IsActive)
            {
                return -2;
            }

            if (await _couponRepository.CheckUserIsBlacklisted(couponCode, userId))
            {
                return -3;
            }

            return coupon.DiscountPercentage;
        }

        [HttpPost("addSalespersonCoupon/{salespersoncouponCode}")]
        public async Task<ActionResult<UserDto>> AddSalesPersonCoupon(string salespersonCouponCode)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            user.CouponCode = salespersonCouponCode;
            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                return Ok(await _mapper.MapUserToUserDto(user));
            }

            return BadRequest("failed to update user");
        }
    }
}