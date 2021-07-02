using System;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatRepository _statRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseRepository _courseRepository;
        private readonly IStatService _statService;
        private readonly IMapperService _mapper;
        public StatsController(IStatRepository statRepository, IStatService statService, IUnitOfWork unitOfWork, ICourseRepository courseRepository, IMapperService mapper)
        {
            _mapper = mapper;
            _statService = statService;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _statRepository = statRepository;
        }

        [HttpGet("set/{code}")]
        public async Task<IActionResult> SetStatByCode(int code)
        {
            var statName = (StatName)code;
            await _statRepository.SetStatByCode(statName);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("get/all/daily")]
        public async Task<IActionResult> GetAllStatsInRange(DateRange dateRange)
        {
            if (dateRange.end.DayOfYear - dateRange.start.DayOfYear > 20)
            {
                return BadRequest();
            }

            var stats = await _statService.GetAllStatsInRange(dateRange.start.Date, dateRange.end.Date);

            return Ok(stats.Select(s => _mapper.MapStatToStatDto(s)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("get/all/total")]
        public async Task<IActionResult> GetAllStatsTotal()
        {
            var res = await _statService.GetAllStatsTotal();
            return Ok(res);
        }

        [HttpGet("set/course/{id}")]
        public async Task<IActionResult> SetCourseStat(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id, true);
            course.Visits++;
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}