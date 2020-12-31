using System.Collections.Generic;
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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>Returns all courses purchased by user.
        /// </summary>
        [HttpGet("Courses/{userId}")]
        public async Task<ActionResult<CourseDto>> GetUserCourses(string userId)
        {
            var courses = await _userRepository.GetUserCourses(userId);
            return Ok(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses));
        }

        /// <summary>Checks the received basket items, and if among them any items have been
        ///purchased before removes it from the basket and returns basket.
        /// </summary>
        [HttpPost("RefineRepetitiveCourses")]
        public async Task<ActionResult<BasketDto>> RefineRepetitiveCourses(BasketDto basketDto)
        {
            var courseIds = await _userRepository.GetUserCourseIds(basketDto.UserId);

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
            var courseIds = await _userRepository.GetUserCourseIds(userId);

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