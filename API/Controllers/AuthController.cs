using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,
        ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("ورود نامعتبر");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (result.IsLockedOut) return Unauthorized("اکانت به مدت پنج دقیقه مسدود شد");
            if (!result.Succeeded) return Unauthorized("ورود نامعتبر");
            var userDto = MapAppUserToUserDto(user);
            return Ok(userDto.Result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return BadRequest("این ایمیل رزرو شده است");
            }

            var appUser = _mapper.Map<RegisterDto, User>(registerDto);
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded) return BadRequest();
            var userDto = await MapAppUserToUserDto(appUser);
            return userDto;
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<UserDto> MapAppUserToUserDto(User user)
        {
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }
    }
}