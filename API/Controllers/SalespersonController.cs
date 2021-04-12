using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Salesperson")]
    public class SalespersonController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapperService _mapper;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;

        public SalespersonController(UserManager<User> userManager,
            IOrderRepository orderRepository,
            IMapperService mapper,
            ICheckoutRepository checkoutRepository,
            IUnitOfWork unitOfWork,
            ICredentialRepository credentialRepository,
            IConfigRepository configRepository,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _checkoutRepository = checkoutRepository;
            _unitOfWork = unitOfWork;
            _credentialRepository = credentialRepository;
            _configRepository = configRepository;
            _userRepository = userRepository;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<PaginatedResult<OrderForSalespersonDto>>> GetOrdersForCheckout([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _orderRepository.GetOrdersForCheckoutAsync(user.CouponCode, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost("checkouts")]
        public async Task<ActionResult<Checkout>> CreateCheckout()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var checkoutThreshold = await _configRepository.GetConfigAsync("DefaultCheckoutThreshold");

            if (!user.CredentialAccepted)
            {
                return BadRequest("not a valid credential");
            }

            if (user.CurrentSalesOfSalesperson < decimal.Parse(checkoutThreshold.Value))
            {
                return BadRequest("less than threshhold");
            }

            var checkout = new Checkout
            {
                UserId = user.Id,
                UserName = user.UserName,
                Status = false,
                AmountToCheckout = user.CurrentSalesOfSalesperson,
                CreatedAt = DateTime.Now
            };

            await _checkoutRepository.CreateCheckoutAsync(checkout);
            user.TotalSalesOfSalesperson += user.CurrentSalesOfSalesperson;
            user.CurrentSalesOfSalesperson = 0;
            await _unitOfWork.CompleteAsync();
            await _userManager.UpdateAsync(user);
            return Ok(checkout);
        }

        [HttpGet("saleamount")]
        public async Task<ActionResult<int>> GetSaleAmount()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(user.CurrentSalesOfSalesperson);
        }

        [HttpGet("totalsaleamount")]
        public async Task<ActionResult<int>> GetTotalSaleAmount()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(user.TotalSalesOfSalesperson);
        }

        [HttpGet("checkouts")]
        public async Task<ActionResult<IEnumerable<Checkout>>> GetCheckouts()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var checkouts = await _checkoutRepository.GetSalespersonCheckoutsAsync(userId);
            return Ok(checkouts);
        }

        [HttpGet("credential")]
        public async Task<ActionResult<SalespersonCredential>> GetCredential()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var credential = await _credentialRepository.GetSalespersonCredentialAsync(userId);
            return Ok(credential);
        }

        [HttpPut("credential")]
        public async Task<ActionResult<SalespersonCredential>> UpdateOrCreateCredential(SalespersonCredential salespersonCredential)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.CredentialAccepted)
            {
                return BadRequest("cant do that anymore!");
            }

            var credential = await _credentialRepository.GetSalespersonCredentialAsync(userId);
            if (credential == null)
            {
                await _credentialRepository.CreateCredentialAsync(salespersonCredential);
                user.SalespersonCredential = salespersonCredential;
                user.CredentialAccepted = false;
            }
            else
            {
                _credentialRepository.UpdateCredential(salespersonCredential);
            }

            await _unitOfWork.CompleteAsync();
            await _userManager.UpdateAsync(user);
            return Ok(credential);
        }

        [HttpGet("credential/Accepted")]
        public async Task<ActionResult<bool>> CheckSalespersonCredentialAccepted()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user.CredentialAccepted == true;
        }

        [HttpGet("info")]
        public async Task<ActionResult<SalespersonDto>> GetInfo()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var salesperson = await _userRepository.FindUserByIdAsync(userId);
            return Ok(_mapper.MapUserToSalespersonDto(salesperson));
        }
    }
}