using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
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
        private readonly IMapperService _mapper;
        private readonly ISMSService _smsService;
        private readonly IConfigRepository _configRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapperService mapper,
            ISMSService smsService,
            IConfigRepository configRepository,
            ICouponRepository couponRepository,
            IUserRepository userRepository)
        {
            _smsService = smsService;
            _configRepository = configRepository;
            _couponRepository = couponRepository;
            _userRepository = userRepository;
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

            var configs = await _configRepository.GetAllConfigsAsync();
            var user = _mapper.MapRegisterDtoToUser(registerDto);

            if (registerDto.CouponCode != null && role.ToUpper() != "SALESPERSON")
            {
                var coupon = await _couponRepository.GetCouponByCode(registerDto.CouponCode);
                if (coupon == null)
                {
                    return BadRequest("coupon not found");
                }

                if (!coupon.IsActive)
                {
                    return BadRequest("coupon is invalid");
                }

                user.Coupon = coupon;
            }
            else if (role.ToUpper() == "SALESPERSON")
            {
                var coupon = new Coupon
                {
                    DiscountPercentage = int.Parse(configs.First(c => c.Title == "DefaultDiscountPercentage").Value),
                    Description = "salesperson coupon",
                    Code = await _couponRepository.GenerateCouponCode(),
                    IsActive = true,

                };

                user.Coupon = coupon;
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("failed to create user");

            var res = role.ToUpper() switch
            {
                "MEMBER" => await _userManager.AddToRoleAsync(user, "Member"),
                "SALESPERSON" => await _userManager.AddToRoleAsync(user, "Salesperson"),
                _ => null
            };

            var userDto = await _mapper.MapUserToUserDto(user);
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
                var res = _smsService.SendSMS(user.PhoneNumber, authToken);
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
                var userDto = await _mapper.MapUserToUserDto(user);
                return Ok(userDto);
            }
        }

        [HttpPost("verifyphone")]
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
            var res = _smsService.SendSMS(verificationDto.PhoneNumber, authToken);
            if (!res)
            {
                return BadRequest("failed to send sms");
            }

            user.VerificationToken = authToken;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("verifytoken")]
        public async Task<ActionResult> VerifyToken(VerificationDto verificationDto)
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

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                await _userManager.UpdateAsync(user);
                var userDto = await _mapper.MapUserToUserDto(user);
                return Ok(userDto);
            }

            user.PhoneNumber = verificationDto.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpGet("phoneexists")]
        public async Task<ActionResult<bool>> CheckPhoneNumberExistsAsync([FromQuery] string phoneNumber)
        {
            return await _userRepository.FindUserByPhoneNumberAsync(phoneNumber) != null;
        }

        [HttpGet("userexists")]
        public async Task<ActionResult<bool>> CheckUserExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }
    }
}