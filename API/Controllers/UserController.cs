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

        [HttpGet("{userId}")]
        public async Task<ActionResult<CourseDto>> GetUserCourses(string userId)
        {
            var courses = await _userService.GetUserCourses(userId);
            return Ok(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses));
        }
    }
}