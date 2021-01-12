using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
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
        private readonly ISMSService _smsService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMapper mapper, ISMSService smsService)
        {
            _smsService = smsService;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber); // used phoneNumber as userName! :|
            if (user == null) return Unauthorized("ورود نامعتبر");
            var authToken = _smsService.GenerateAuthToken();
            var res = _smsService.SendSMS(user.PhoneNumber, authToken);

            if (!res)
            {
                return BadRequest("ارسال پیامک با مشکل روبرو شد");
            }

            user.VerificationToken = authToken;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<RegisterDto, User>(registerDto);
            var authToken = _smsService.GenerateAuthToken();
            var res = _smsService.SendSMS(user.PhoneNumber, authToken);
            if (!res)
            {
                return BadRequest("ارسال پیامک با مشکل روبرو شد");
            }

            user.VerificationToken = authToken;
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest("ساخت اکانت با مشکل روبرو شد");
            await _userManager.AddToRoleAsync(user, "Member");

            return Ok();
        }

        [HttpPost("smsverification")]
        public async Task<ActionResult> SMSVerification(VerificationDto verificationDto)
        {
            var user = await _userManager.FindByNameAsync(verificationDto.PhoneNumber); // used phoneNumber as userName!
            if (verificationDto.AuthToken != user.VerificationToken)
            {
                return Unauthorized("not authorized!");
            }

            user.VerificationToken = "";
            await _userManager.UpdateAsync(user);
            var userDto = MapAppUserToUserDto(user);
            return Ok(userDto.Result);
        }

        [HttpGet("phoneexists")]
        public async Task<ActionResult<bool>> CheckPhoneNumberExistsAsync([FromQuery] string phoneNumber)
        {
            return await _userManager.FindByNameAsync(phoneNumber) != null;
        }

        public async Task<UserDto> MapAppUserToUserDto(User user)
        {
            return new UserDto
            {
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }
    }
}