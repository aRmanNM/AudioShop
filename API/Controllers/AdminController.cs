using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost("CreateCourse")]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            var newCourse = await _adminRepository.CreateCourse(course);
            return newCourse;
        }

        [HttpPost("CreateEpisode")]
        public async Task<ActionResult<CourseEpisode>> CreateEpisode(CourseEpisode courseEpisode)
        {
            var newCourseEpisode = await _adminRepository.CreateEpisode(courseEpisode);
            return newCourseEpisode;
        }

        [HttpPost("DeleteCourse")]
        public async Task<ActionResult> DeleteCourse(IEnumerable<Course> courses)
        {
            await _adminRepository.DeleteCourses(courses);
            return Ok();
        }

        [HttpPost("DeleteEpisode")]
        public async Task<ActionResult> DeleteEpisode(IEnumerable<CourseEpisode> courseEpisodes)
        {
            await _adminRepository.DeleteEpisodes(courseEpisodes);
            return Ok();
        }

        [HttpPost("UpdateCourse")]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            var updatedCourse = await _adminRepository.UpdateCourse(course);
            return updatedCourse;
        }

        [HttpPost("UpdateEpisode")]
        public async Task<ActionResult<CourseEpisode>> UpdateEpisode(CourseEpisode courseEpisode)
        {
            var updatedcourseEpisode = await _adminRepository.UpdateEpisode(courseEpisode);
            return updatedcourseEpisode;
        }
    }
}