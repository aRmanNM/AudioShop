using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapperService _mapper;

        public CoursesController(ICourseRepository courseRepository,
            IEpisodeRepository episodeRepository,
            IMapperService mapper)
        {
            _courseRepository = courseRepository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses([FromQuery] bool includeEpisodes = false)
        {
            var courses = await _courseRepository.GetCourses(includeEpisodes);
            var courseDtos = courses.Select(c => _mapper.MapCourseToCourseDto(c));
            return Ok(courseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [HttpGet("{courseId}/episodes")]
        public async Task<ActionResult<List<EpisodeDto>>> GetCourseEpisodes(int courseId)
        {
            var episodes = await _episodeRepository.GetCourseEpisodes(courseId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e));
            return Ok(episodeDtos);
        }

        [HttpGet("episode/{id}")]
        public async Task<ActionResult<EpisodeDto>> GetEpisodeById(int id)
        {
            var episode = await _episodeRepository.GetEpisodeById(id);
            return Ok(_mapper.MapEpisodeToEpisodeDto(episode));
        }
    }
}