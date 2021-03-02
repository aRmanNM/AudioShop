using System.Collections.Generic;
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
        public async Task<ActionResult<List<CourseDto>>> GetCourses(bool includeEpisodes = false,
            string search = null, bool includeInactive = false, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _courseRepository.GetCourses(includeEpisodes, search, includeInactive, pageNumber, pageSize);
            var resultWithDtos = new PaginatedResult<CourseDto>();
            resultWithDtos.TotalItems = result.TotalItems;
            resultWithDtos.Items = result.Items.Select(c => _mapper.MapCourseToCourseDto(c));
            return Ok(resultWithDtos);
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
        [HttpPut]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            var updatedCourse = _courseRepository.UpdateCourse(course);
            await _unitOfWork.CompleteAsync();
            return updatedCourse;
        }

        //
        // REVIEWS
        //

        [HttpGet("{courseId}/reviews")]
        public async Task<ActionResult<List<Review>>> GetCourseReviews(int courseId, bool accepted = true)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role.ToUpper() != "ADMIN")
            {
                accepted = true;
            }

            var reviews = await _reviewRepository.GetCourseReviews(courseId, accepted);
            return Ok(reviews);
        }

        [Authorize(Roles = "Member")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("reviews")]
        public async Task<ActionResult<List<Review>>> GetAllReviews([FromQuery] bool accepted)
        {
            var reviews = await _reviewRepository.GetAllReviews(accepted);
            return Ok(reviews);
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{courseId}/episodes")]
        public async Task<ActionResult> UpdateCourseEpisodes(Episode[] episodes)
        {
            _episodeRepository.UpdateEpisodes(episodes);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}