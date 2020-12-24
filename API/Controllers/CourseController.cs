using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;
        public CourseController(IStoreService storeService, IMapper mapper)
        {
            _mapper = mapper;
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses()
        {
            var courses = await _storeService.GetCourses();
            return Ok(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _storeService.GetCourseById(id);
            return Ok(_mapper.Map<Course, CourseDto>(course));
        }

        [HttpGet("episodes/{courseId}")]
        public async Task<ActionResult<List<CourseEpisodeDto>>> GetCourseEpisodes(int courseId)
        {
            var courses = await _storeService.GetCourseEpisodes(courseId);
            return Ok(_mapper.Map<IEnumerable<CourseEpisode>, IEnumerable<CourseEpisodeDto>>(courses));
        }

        [HttpGet("episode/{id}")]
        public async Task<ActionResult<CourseEpisodeDto>> GetCourseEpisodeById(int id)
        {
            var courses = await _storeService.GetCourseEpisodeById(id);
            return Ok(_mapper.Map<CourseEpisode, CourseEpisodeDto>(courses));
        }
    }
}