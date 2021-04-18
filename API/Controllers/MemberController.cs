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
    [Authorize(Roles = "Member")]
    public class MemberController : ControllerBase
    {
        private readonly IMapperService _mapper;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly UserManager<User> _userManager;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProgressRepository _progressRepository;

        public MemberController(
            IMapperService mapper,
            IEpisodeRepository episodeRepository,
            ICouponRepository couponRepository,
            UserManager<User> userManager,
            IFavoriteRepository favoriteRepository,
            IUnitOfWork unitOfWork,
            IProgressRepository progressRepository)
        {
            _mapper = mapper;
            _episodeRepository = episodeRepository;
            _couponRepository = couponRepository;
            _userManager = userManager;
            _favoriteRepository = favoriteRepository;
            _unitOfWork = unitOfWork;
            _progressRepository = progressRepository;
        }

        [HttpGet("Episodes/{userId}")]
        public async Task<ActionResult<EpisodeDto>> GetUserEpisodes(string userId)
        {
            var episodes = await _episodeRepository.GetUserEpisodesAsync(userId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e)).ToArray();
            return Ok(episodeDtos);
        }

        [HttpPost("refinebasket")]
        public async Task<ActionResult<BasketDto>> Refinebasket(BasketDto basketDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEpisodeIds = await _episodeRepository.GetUserEpisodeIdsAsync(userId);

            // TODO: Check this for validity
            var filteredEpisdeIds = basketDto.EpisodeIds.Where(epi => !userEpisodeIds.Any(uepi => uepi == epi)).ToArray();
            basketDto.EpisodeIds = filteredEpisdeIds;

            return basketDto;
        }

        [HttpGet("CheckIfEpisodeIsRepetitive/{userId}/{episodeId}")]
        public async Task<ActionResult<bool>> CheckIfCourseIsRepetitive(string userId, int episodeId)
        {
            var episodeIds = await _episodeRepository.GetUserEpisodeIdsAsync(userId);

            foreach (var tempEpisodeId in episodeIds)
            {
                if (tempEpisodeId == episodeId)
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
            var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode);

            if (coupon == null)
            {
                return -1;
            }

            if (!coupon.IsActive)
            {
                return -2;
            }

            if (await _couponRepository.CheckUserIsBlacklistedAsync(couponCode, userId))
            {
                return -3;
            }

            if (!string.IsNullOrEmpty(coupon.UserId) && coupon.IsActive == true)
            {
                return -4;
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
                return Ok(await _mapper.MapUserToUserDtoAsync(user));
            }

            return BadRequest("failed to update user");
        }

        #region Favorites

        [HttpGet("favorites")]
        public async Task<ActionResult<Favorite>> GetFavorites()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var favorites = await _favoriteRepository.GetFavorites(userId);
            return Ok(favorites);
        }

        [HttpPost("favorites")]
        public async Task<ActionResult<Favorite>> CreateFavorite(Favorite favorite)
        {
            await _favoriteRepository.CreateFavorite(favorite);
            await _unitOfWork.CompleteAsync();
            return Ok(favorite);
        }

        [HttpPut("favorites/{id}")]
        public async Task<ActionResult<Favorite>> UpdateFavorite(Favorite favorite)
        {
            _favoriteRepository.UpdateFavorite(favorite);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete("favorites/{id}")]
        public async Task<ActionResult<Favorite>> DeleteFavorite(int id)
        {
            await _favoriteRepository.DeleteFavorite(id);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        #endregion

        #region Progress

        [HttpGet("{courseId}/progress")]
        public async Task<ActionResult<Progress>> GetCourseProgressForMember(int courseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var progress = await _progressRepository.GetCourseProgress(userId, courseId);
            return Ok(progress);
        }

        [HttpPost("{courseId}/progress")]
        public async Task<ActionResult<Progress>> CreateProgress(int courseId, Progress progress)
        {
            await _progressRepository.CreateProgress(progress);
            await _unitOfWork.CompleteAsync();
            return Ok(progress);
        }

        [HttpPut("{courseId}/progress/{progressId}")]
        public async Task<ActionResult<Favorite>> UpdateProgress(int courseId, int progressId, Progress progress)
        {
            _progressRepository.UpdateProgress(progress);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete("{courseId}/progress/{progressId}")]
        public async Task<ActionResult<Favorite>> DeleteProgress(int courseId, int progressId)
        {
            await _progressRepository.DeleteProgress(progressId);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        #endregion


    }
}