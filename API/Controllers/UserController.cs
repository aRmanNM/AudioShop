using System.Collections.Generic;
using System.Threading.Tasks;
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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>Returns all courses purchased by user.
        /// </summary>
        [HttpGet("Courses/{userId}")]
        public async Task<ActionResult<CourseDto>> GetUserCourses(string userId)
        {
            var courses = await _userService.GetUserCourses(userId);
            return Ok(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses));
        }

        /// <summary>Checks the received basket items, and if among them any items have been
        ///purchased before removes it from the basket and returns basket.
        /// </summary>
        [HttpGet("RefineRepetitiveCourses")]
        public async Task<ActionResult<BasketDto>> RefineRepetitiveCourses(BasketDto basketDto)
        {
            var courseIds = await _userService.GetUserCourseIds(basketDto.UserId);

            foreach(var courseId in courseIds)
            {
                foreach(var course in basketDto.CourseDtos)
                {
                    if(courseId == course.Id)
                    {
                        basketDto.CourseDtos.Remove(course);
                    }
                }
            }

            return basketDto;
        }

        /// <summary>Returns true if the selected course has been purchased by the user before.
        /// </summary>
        [HttpGet("CheckIfCourseIsRepetitive/{userId}/{courseId}")]
        public async Task<ActionResult<bool>> CheckIfCourseIsRepetitive(string userId, int courseId)
        {
            var courseIds = await _userService.GetUserCourseIds(userId);

            foreach(var tempCourseId in courseIds)
            {
                if(tempCourseId == courseId){
                    return true;
                }
            }

            return false;
        }
    }
}