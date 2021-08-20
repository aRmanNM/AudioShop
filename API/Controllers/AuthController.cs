using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapperService _mapper;
        private readonly ISMSService _smsService;
        private readonly IConfigRepository _configRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapperService mapper,
            ISMSService smsService,
            IConfigRepository configRepository,
            ICouponRepository couponRepository,
            IUserRepository userRepository,
            IConfiguration config)
        {
            _smsService = smsService;
            _configRepository = configRepository;
            _couponRepository = couponRepository;
            _userRepository = userRepository;
            _config = config;
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, [FromQuery] string role = "member")
        {
            if (string.IsNullOrEmpty(registerDto.PhoneNumber))
            {
                var phoneExists = await _userRepository.FindUserByPhoneNumberAsync(registerDto.PhoneNumber) != null;
                if (phoneExists)
                {
                    return BadRequest("cant use this phone");
                }
            }

            var configs = await _configRepository.GetConfigsByGroupAsync("General");
            var user = _mapper.MapRegisterDtoToUser(registerDto);
            user.UserType = role.ToUpper() switch
            {
                "SALESPERSON" => UserType.Salesperson,
                "MEMBER" => UserType.Member,
                _ => UserType.Other
            };

            if (registerDto.SalespersonCouponCode != null && role.ToUpper() != "SALESPERSON")
            {
                user.CouponCode = registerDto.SalespersonCouponCode;
            }
            else if (role.ToUpper() == "SALESPERSON")
            {
                var coupon = new Coupon
                {
                    DiscountPercentage = int.Parse(configs.First(c => c.TitleEn == "DefaultDiscountPercentage").Value),
                    Description = "salesperson coupon",
                    Code = await _couponRepository.GenerateCouponCodeAsync(),
                    IsActive = true,
                };

                user.CouponCode = coupon.Code;
                user.Coupon = coupon;
                user.SalePercentageOfSalesperson = int.Parse(configs.First(c => c.TitleEn == "DefaultSalespersonSharePercentage").Value);
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("failed to create user");

            var res = role.ToUpper() switch
            {
                "MEMBER" => await _userManager.AddToRoleAsync(user, "Member"),
                "SALESPERSON" => await _userManager.AddToRoleAsync(user, "Salesperson"),
                _ => null
            };

            var userDto = await _mapper.MapUserToUserDtoAsync(user);
            return Ok(userDto);
        }

        [HttpPost("updateUser")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserUpdateDto userUpdateDto, [FromQuery] string role = "member")
        {
            var configs = await _configRepository.GetConfigsByGroupAsync("General");
            var user = await _userRepository.FindUserByIdAsync(userUpdateDto.UserId);

            var updateProps = userUpdateDto.GetType().GetProperties();
            foreach (var prop in updateProps)
            {
                user.GetType().GetProperty(prop.Name)?.SetValue(user, prop.GetValue(userUpdateDto));
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("failed to create user");


            var userDto = await _mapper.MapUserToUserDtoAsync(user);
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto, [FromQuery] bool usingPhone = false)
        {
            if (usingPhone)
            {
                var user = await _userRepository.FindUserByPhoneNumberAsync(loginDto.PhoneNumber);
                if (user == null)
                {
                    return BadRequest("user not found");
                }

                var authToken = _smsService.GenerateAuthToken();
                var res = _smsService.SendVerificationSMS(user.PhoneNumber, authToken);
                if (!res)
                {
                    return BadRequest("failed to send sms");
                }

                user.VerificationToken = authToken;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            else
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    return BadRequest("user not found");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
                if (result.IsLockedOut) return Unauthorized("account locked");
                if (!result.Succeeded) return Unauthorized("not authorized");
                var userDto = await _mapper.MapUserToUserDtoAsync(user);
                return Ok(userDto);
            }
        }

        [HttpPost("verifyPhone")]
        public async Task<ActionResult> VerifyPhone(VerificationDto verificationDto)
        {
            var phoneExists = await _userRepository.FindUserByPhoneNumberAsync(verificationDto.PhoneNumber) != null;
            if (phoneExists)
            {
                return BadRequest("cant use this phone");
            }

            var user = await _userManager.FindByIdAsync(verificationDto.UserId);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var authToken = _smsService.GenerateAuthToken();
            var res = _smsService.SendVerificationSMS(verificationDto.PhoneNumber, authToken);
            if (!res)
            {
                return BadRequest("failed to send sms");
            }

            user.VerificationToken = authToken;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("verifyToken")]
        public async Task<ActionResult<UserDto>> VerifyToken(VerificationDto verificationDto)
        {
            var user = !string.IsNullOrEmpty(verificationDto.UserId) ?
                await _userManager.FindByIdAsync(verificationDto.UserId) :
                await _userRepository.FindUserByPhoneNumberAsync(verificationDto.PhoneNumber);

            if (user == null)
            {
                return BadRequest("user not found");
            }

            if (verificationDto.AuthToken != user.VerificationToken)
            {
                return Unauthorized("not authorized");
            }

            user.VerificationToken = "";

            user.PhoneNumber = verificationDto.PhoneNumber;
            user.PhoneNumberConfirmed = true;
            var userDto = await _mapper.MapUserToUserDtoAsync(user);

            await _userManager.UpdateAsync(user);
            return Ok(userDto);
        }

        [HttpGet("phoneexists")]
        public async Task<ActionResult<bool>> CheckPhoneNumberExistsAsync([FromQuery] string phoneNumber)
        {
            return await _userRepository.FindUserByPhoneNumberAsync(phoneNumber) != null;
        }

        [HttpGet("userexists")]
        public async Task<ActionResult<bool>> CheckUserExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName.ToLower()) != null;
        }

        [Authorize]
        [HttpPut("updatepassword")]
        public async Task<ActionResult> UpdatePassword(PasswordChangeDto passwordChangeDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var res = await _userManager.ChangePasswordAsync(user, passwordChangeDto.OldPassword, passwordChangeDto.NewPassword);
            if (!res.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        // TODO: Don't keep this bullshit.
        [HttpGet("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromQuery] string userName, [FromQuery] string newPassword, [FromQuery] string secret)
        {
            if (secret != _config["Pass:Secret"])
            {
                return BadRequest("cant do that!");
            }

            var user = await _userManager.FindByNameAsync(userName.ToLower());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, newPassword ?? _config["Pass:Default"]);

            if (!res.Succeeded)
            {
                return BadRequest("failed");
            }

            return Ok();
        }
    }
}