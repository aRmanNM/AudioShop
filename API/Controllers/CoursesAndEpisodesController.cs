using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _host;
        private readonly IFileService _fileService;

        public CoursesController(ICourseRepository courseRepository,
            IEpisodeRepository episodeRepository,
            IMapperService mapper,
            IUnitOfWork unitOfWork,
            IReviewRepository reviewRepository,
            IWebHostEnvironment host,
            IFileService fileService)
        {
            _courseRepository = courseRepository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _host = host;
            _fileService = fileService;
        }

        #region Courses

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CourseDto>>> GetCourses(bool includeEpisodes = false,
            string search = null, bool includeInactive = false, int pageNumber = 1, int pageSize = 10,
            string category = null, CourseType courseType = CourseType.None, bool onlyFeatured = false)
        {
            var result = await _courseRepository.GetCoursesAsync(includeEpisodes, search, includeInactive, pageNumber, pageSize, category, courseType, onlyFeatured);
            var resultWithDtos = new PaginatedResult<CourseDto>();
            resultWithDtos.TotalItems = result.TotalItems;
            resultWithDtos.Items = result.Items.Select(c => _mapper.MapCourseToCourseDto(c));
            return Ok(resultWithDtos);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<CourseDto>> GetFeaturedCourses(CourseType courseType = CourseType.Course, int count = 10)
        {
            var courses = await _courseRepository.GetFeaturedCoursesAsync(courseType, count);
            return Ok(courses.Select(c => _mapper.MapCourseToCourseDto(c)));
        }

        [HttpGet("topsellers")]
        public async Task<ActionResult<CourseDto>> GetTopSellersCourses(CourseType courseType = CourseType.Course, int count = 10)
        {
            var courses = await _courseRepository.GetTopSellersCoursesAsync(courseType, count);
            return Ok(courses.Select(c => _mapper.MapCourseToCourseDto(c)));
        }


        [HttpGet("topclicked")]
        public async Task<ActionResult<CourseDto>> GetTopٰClickedCoursesAsync(CourseType courseType = CourseType.Course, int count = 10)
        {
            var courses = await _courseRepository.GetTopٰClickedCoursesAsync(courseType, count);
            return Ok(courses.Select(c => _mapper.MapCourseToCourseDto(c)));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CourseDto courseDto)
        {
            var course = _mapper.MapCourseDtoToCourse(courseDto);

            course.CourseCategories = courseDto.Categories.Select(c => new CourseCategory
            {
                CourseId = course.Id,
                CategoryId = c.Id
            }).ToList();

            await _courseRepository.CreateCourseAsync(course);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            course.LastEdited = DateTime.Now;
            var courseToUpdate = _courseRepository.UpdateCourse(course);
            await _courseRepository.DeleteCourseCategories(course.Id);
            await _unitOfWork.CompleteAsync();

            var courseCategories = course.Categories.Select(cc => new CourseCategory
            {
                CourseId = course.Id,
                CategoryId = cc.Id
            }).ToArray();

            await _courseRepository.AdddCourseCategories(courseCategories);
            await _unitOfWork.CompleteAsync();

            return courseToUpdate;
        }

        #endregion

        #region Reviews

        [HttpGet("{courseId}/reviews")]
        public async Task<ActionResult<PaginatedResult<ReviewDto>>> GetCourseReviews(int courseId, bool accepted = true, int pageNumber = 1, int pageSize = 10)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null || (role != null && role.ToUpper() != "ADMIN"))
            {
                accepted = true;
            }

            var reviews = await _reviewRepository.GetCourseReviewsAsync(courseId, accepted, pageNumber, pageSize);
            return Ok(reviews);
        }

        [Authorize(Roles = "Member")]
        [HttpPost("{courseId}/reviews")]
        public async Task<ActionResult<ReviewDto>> AddCourseReview(int courseId, Review review)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            review.Accepted = false;
            review.UserId = userId;
            review.CourseId = courseId;

            await _reviewRepository.AddReviewAsync(review);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{courseId}/reviews/{reviewId}")]
        public async Task<ActionResult<ReviewDto>> UpdateCourseReview(int courseId, int reviewId, ReviewDto reviewDto)
        {
            await _reviewRepository.UpdateReview(reviewDto);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("reviews")]
        public async Task<ActionResult> BulkActionOnReviews(int[] reviewIds, string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                return BadRequest();
            }

            switch (action)
            {
                case "toggle":
                    await _reviewRepository.ToggleMultipleReviews(reviewIds);
                    break;
                case "delete":
                    _reviewRepository.DeleteMultipleReviews(reviewIds);
                    break;
                default: break;
            }

            await _unitOfWork.CompleteAsync();
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("reviews")]
        public async Task<ActionResult<PaginatedResult<ReviewDto>>> GetAllReviews([FromQuery] bool accepted, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _reviewRepository.GetAllReviewsAsync(accepted, pageNumber, pageSize);
            return Ok(result);
        }

        #endregion

        #region Episodes

        [HttpGet("{courseId}/episodes")]
        public async Task<ActionResult<List<EpisodeDto>>> GetCourseEpisodes(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound("course not found!");
            }

            var episodes = await _episodeRepository.GetCourseEpisodesAsync(courseId);
            var episodeDtos = episodes.Select(e => _mapper.MapEpisodeToEpisodeDto(e));
            return Ok(episodeDtos);
        }

        [HttpGet("episodes/{id}")]
        public async Task<ActionResult<EpisodeDto>> GetEpisodeById(int id)
        {
            var episode = await _episodeRepository.GetEpisodeByIdAsync(id);
            var episodeDto = _mapper.MapEpisodeToEpisodeDto(episode);
            return Ok(episodeDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("episodes")]
        public async Task<ActionResult<EpisodeDto>> CreateEpisode(EpisodeDto episodeDto)
        {
            var episode = _mapper.MapEpisodeDtoToEpisode(episodeDto);
            await _episodeRepository.CreateEpisodeAsync(episode);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapEpisodeToEpisodeDto(episode));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("episodes/{id}")]
        public async Task<ActionResult> DeleteEpisode(int id)
        {
            /*
                - check if episode is bought by someone
                - delete audio files
                - delete episode
            */

            var episode = await _episodeRepository.GetEpisodeByIdAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            var isBought = await _episodeRepository.CheckIfAlreadyBoughtAsync(episode);

            if (isBought)
            {
                return BadRequest("already bought");
            }

            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Files", episode.CourseId.ToString());
            try
            {
                foreach (var item in episode.Audios)
                {
                    _fileService.Delete(item.FileName, uploadFolderPath);
                }
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

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

        #endregion
    }
}