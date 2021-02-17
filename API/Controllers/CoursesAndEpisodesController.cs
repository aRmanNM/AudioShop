using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;

        public CoursesController(ICourseRepository courseRepository,
            IEpisodeRepository episodeRepository,
            IMapperService mapper,
            IUnitOfWork unitOfWork,
            IReviewRepository reviewRepository)
        {
            _courseRepository = courseRepository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
        }

        //
        // COURSES
        //

        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses(bool includeEpisodes = false, string search = null)
        {
            var courses = await _courseRepository.GetCourses(includeEpisodes, search);
            var courseDtos = courses.Select(c => _mapper.MapCourseToCourseDto(c));
            return Ok(courseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CourseDto courseDto)
        {
            var course = _mapper.MapCourseDtoToCourse(courseDto);
            await _courseRepository.CreateCourse(course);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(IEnumerable<Course> courses)
        {
            _courseRepository.DeleteCourses(courses);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            var updatedCourse = _courseRepository.UpdateCourse(course);
            await _unitOfWork.CompleteAsync();
            return updatedCourse;
        }

        [HttpGet("{courseId}/reviews")]
        public async Task<ActionResult<List<Review>>> GetCourseReviews(int courseId)
        {
            var reviews = await _reviewRepository.GetCourseReviews(courseId);
            return Ok(reviews);
        }

        [Authorize(Roles="Member")]
        [HttpPost("{courseId}/reviews")]
        public async Task<ActionResult<Review>> AddCourseReview(int courseId, Review review)
        {
            if (courseId != review.CourseId)
            {
                return BadRequest();
            }

            review.Accepted = false;
            await _reviewRepository.AddReview(review);
            await _unitOfWork.CompleteAsync();
            return Ok(review);
        }

        [HttpPut("{courseId}/reviews/{reviewId}")]
        public async Task<ActionResult<Review>> UpdateCourseReview(int courseId, int reviewId, Review review)
        {
            if (courseId != review.CourseId)
            {
                return BadRequest();
            }

            if (reviewId != review.Id)
            {
                return BadRequest();
            }

            _reviewRepository.UpdateReview(review);
            await _unitOfWork.CompleteAsync();
            return Ok(review);
        }

        //
        // EPISODES
        //

        [HttpGet("{courseId}/episodes")]
        public async Task<ActionResult<List<EpisodeDto>>> GetCourseEpisodes(int courseId)
        {
            var course = await _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                return NotFound("course not found!");
            }

            var episodes = await _episodeRepository.GetCourseEpisodes(courseId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e));
            return Ok(episodeDtos);
        }

        [HttpGet("episodes/{id}")]
        public async Task<ActionResult<EpisodeDto>> GetEpisodeById(int id)
        {
            var episode = await _episodeRepository.GetEpisodeById(id);
            var episodeDto = _mapper.MapEpisodeToEpisodeDto(episode);
            return Ok(episodeDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("episodes")]
        public async Task<ActionResult<EpisodeDto>> CreateEpisode(EpisodeDto episodeDto)
        {
            var episode = _mapper.MapEpisodeDtoToEpisode(episodeDto);
            await _episodeRepository.CreateEpisode(episode);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapEpisodeToEpisodeDto(episode));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("episodes/{id}")]
        public async Task<ActionResult> DeleteEpisode(Episode episode)
        {
            _episodeRepository.DeleteEpisode(episode);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("episodes")]
        public async Task<ActionResult<EpisodeDto>> UpdateEpisode(Episode episode)
        {
            _episodeRepository.UpdateEpisode(episode);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapEpisodeToEpisodeDto(episode));
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{courseId}/episodes")]
        public async Task<ActionResult> UpdateCourseEpisodes(Episode[] episodes)
        {
            _episodeRepository.UpdateEpisodes(episodes);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}