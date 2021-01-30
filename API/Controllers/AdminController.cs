using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;

        public AdminController(UserManager<User> userManager,
            ICheckoutRepository checkoutRepository,
            ICourseRepository courseRepository,
            IEpisodeRepository episodeRepository,
            IMapperService mapper,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment host)
        {
            _userManager = userManager;
            _checkoutRepository = checkoutRepository;
            _courseRepository = courseRepository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _host = host;
        }

        //
        // MEMBERS
        //
        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<User>>> GetMembers()
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            return Ok(members);
        }

        [HttpGet("members/{id}")]
        public async Task<ActionResult<User>> UpdateMember(User member)
        {
            var res = await _userManager.UpdateAsync(member);
            if (!res.Succeeded)
            {
                return BadRequest("failed to edit user");
            }

            return Ok(member);
        }

        //
        // SALESPERSONS
        //


        //
        // COURSES
        //
        [HttpPost("courses")]
        public async Task<ActionResult<Course>> CreateCourse(CourseDto courseDto)
        {
            var course = _mapper.MapCourseDtoToCourse(courseDto);
            await _courseRepository.CreateCourse(course);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.MapCourseToCourseDto(course));
        }

        [HttpDelete("courses")]
        public async Task<ActionResult> DeleteCourse(IEnumerable<Course> courses)
        {
            _courseRepository.DeleteCourses(courses);
            return Ok();
        }

        [HttpPut("UpdateCourse")]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            var updatedCourse = _courseRepository.UpdateCourse(course);
            return updatedCourse;
        }

        //
        // EPISODES
        //
        [HttpPost("CreateEpisode")]
        public async Task<ActionResult<Episode>> CreateEpisode(Episode courseEpisode)
        {
            var newCourseEpisode = await _episodeRepository.CreateEpisode(courseEpisode);
            return newCourseEpisode;
        }

        [HttpPost("DeleteEpisode")]
        public async Task<ActionResult> DeleteEpisode(IEnumerable<Episode> courseEpisodes)
        {
            _episodeRepository.DeleteEpisodes(courseEpisodes);
            return Ok();
        }

        [HttpPost("UpdateEpisode")]
        public async Task<ActionResult<Episode>> UpdateEpisode(Episode courseEpisode)
        {
            var updatedcourseEpisode = _episodeRepository.UpdateEpisode(courseEpisode);
            return updatedcourseEpisode;
        }

        //
        // ORDERS
        //

        //
        // CHECKOUTS
        //
        [HttpGet("checkouts")]
        public async Task<ActionResult<IEnumerable<Checkout>>> GetCheckouts(bool status)
        {
            var checkouts = await _checkoutRepository.GetCheckouts(status);
            return Ok(checkouts);
        }

        [HttpPut("checkouts/{id}")]
        public async Task<ActionResult<Checkout>> EditCheckout(Checkout checkout)
        {
            checkout.PaidAt = DateTime.Now;
            _checkoutRepository.EditCheckout(checkout);
            return Ok(checkout);
        }
    }
}