using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CourseController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;
        public CourseController(IStoreRepository storeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
        }

        // TODO:
        // pagination
        //
        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses()
        {
            var courses = await _storeRepository.GetCourses();
            return Ok(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _storeRepository.GetCourseById(id);
            return Ok(_mapper.Map<Course, CourseDto>(course));
        }

        [HttpGet("episodes/{courseId}")]
        public async Task<ActionResult<List<CourseEpisodeDto>>> GetCourseEpisodes(int courseId)
        {
            var courses = await _storeRepository.GetCourseEpisodes(courseId);
            return Ok(_mapper.Map<IEnumerable<CourseEpisode>, IEnumerable<CourseEpisodeDto>>(courses));
        }

        [HttpGet("episode/{id}")]
        public async Task<ActionResult<CourseEpisodeDto>> GetCourseEpisodeById(int id)
        {
            var courses = await _storeRepository.GetCourseEpisodeById(id);
            return Ok(_mapper.Map<CourseEpisode, CourseEpisodeDto>(courses));
        }
    }
}