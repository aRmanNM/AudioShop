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

        public AuthController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            ISMSService smsService)
        {
            _smsService = smsService;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto, [FromQuery] string role = "member")
        {
            if (role.ToUpper() == "MEMBER")
            {
                var user = await _smsService.FindUserByPhoneNumberAsync(loginDto.PhoneNumber);
                if (user == null)
                {
                    return BadRequest("کاربری پیدا نشد");
                }
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
            else if (role.ToUpper() == "SALESPERSON" || role.ToUpper() == "ADMIN")
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    return BadRequest("کاربری پیدا نشد");
                }
                if (user == null) return Unauthorized("ورود نامعتبر");
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
                if (result.IsLockedOut) return Unauthorized("اکانت به مدت پنج دقیقه مسدود شد");
                if (!result.Succeeded) return Unauthorized("ورود نامعتبر");
                var userDto = await MapUserToUserDto(user);
                return Ok(userDto);
            }

            return BadRequest("درخواست تامعتبر است");

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, [FromQuery] string role = "member")
        {
            var user = _mapper.Map<RegisterDto, User>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("ساخت اکانت با مشکل روبرو شد");
            var res = role.ToUpper() switch
            {
                "MEMBER" => await _userManager.AddToRoleAsync(user, "Member"),
                "SALESPERSON" => await _userManager.AddToRoleAsync(user, "SalesPerson"),
                _ => null
            };

            var userDto = await MapUserToUserDto(user);
            return Ok(userDto);
        }

        [HttpPost("verifyphone")]
        public async Task<ActionResult> VerifyPhone(VerificationDto verificationDto)
        {
            var user = await _userManager.FindByIdAsync(verificationDto.UserId);
            if (user == null)
            {
                return NotFound("کاربری پیدا نشد");
            }

            var authToken = _smsService.GenerateAuthToken();
            var res = _smsService.SendSMS(verificationDto.PhoneNumber, authToken);
            if (!res)
            {
                return BadRequest("ارسال پیامک با مشکل روبرو شد");
            }

            user.VerificationToken = authToken;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("verifytoken")]
        public async Task<ActionResult> VerifyToken(VerificationDto verificationDto)
        {
            var user = await _smsService.FindUserByPhoneNumberAsync(verificationDto.PhoneNumber);
            if (user == null)
            {
                return BadRequest("کاربر پیدا نشد");
            }

            if (verificationDto.AuthToken != user.VerificationToken)
            {
                return Unauthorized("اعتبار سنجی ناموفق");
            }

            user.VerificationToken = "";
            if (user.PhoneNumber != null)
            {
                await _userManager.UpdateAsync(user);
                var userDto = await MapUserToUserDto(user);
                return Ok(userDto);
            }

            user.PhoneNumber = verificationDto.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpGet("phoneexists")]
        public async Task<ActionResult<bool>> CheckPhoneNumberExistsAsync([FromQuery] string phoneNumber)
        {
            return await _smsService.FindUserByPhoneNumberAsync(phoneNumber) != null;
        }

        [HttpGet("userexists")]
        public async Task<ActionResult<bool>> CheckUserExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<UserDto> MapUserToUserDto(User user)
        {
            return new UserDto
            {
                Token = await _tokenService.CreateToken(user),
                HasPhoneNumber = (user.PhoneNumber == null) ? false : true
            };
        }
    }
}