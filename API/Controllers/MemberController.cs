using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
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

        public MemberController(
            IMapperService mapper,
            IEpisodeRepository episodeRepository,
            ICouponRepository couponRepository)
        {
            _mapper = mapper;
            _episodeRepository = episodeRepository;
            _couponRepository = couponRepository;
        }

        [HttpGet("Episodes/{userId}")]
        public async Task<ActionResult<EpisodeDto>> GetUserEpisodes(string userId)
        {
            var episodes = await _episodeRepository.GetUserEpisodes(userId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e)).ToArray();
            return Ok(episodeDtos);
        }

        [HttpPost("refinebasket")]
        [AllowAnonymous]
        public async Task<ActionResult<BasketDto>> Refinebasket(BasketDto basketDto)
        {
            var episodeIds = await _episodeRepository.GetUserEpisodeIds(basketDto.UserId);

            foreach(var id in episodeIds)
            {
                for(int i = 0; i < basketDto.Episodes.Count; i++)
                {
                    var episodeDto = basketDto.Episodes.FirstOrDefault(x => x.Id != 0);
                    if (id != episodeDto.Id) continue;
                    basketDto.Episodes.Remove(episodeDto);
                }
            }

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

        [HttpGet("isBlacklisted/{couponCode}")]
        public async Task<bool> CheckMemberIsBlacklisted(string couponCode)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _couponRepository.CheckUserIsBlacklisted(couponCode, userId);
        }
    }
}