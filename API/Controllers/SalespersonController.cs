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

        public SalespersonController(UserManager<User> userManager,
            IOrderRepository orderRepository,
            IMapperService mapper,
            ICheckoutRepository checkoutRepository,
            IUnitOfWork unitOfWork,
            ICredentialRepository credentialRepository,
            IConfigRepository configRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _checkoutRepository = checkoutRepository;
            _unitOfWork = unitOfWork;
            _credentialRepository = credentialRepository;
            _configRepository = configRepository;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderForSalespersonDto>>> GetOrdersForCheckout()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var ordersForSalesperson = await _orderRepository.GetOrdersForCheckout(user.CouponCode);
            return Ok(ordersForSalesperson);
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

            await _checkoutRepository.CreateCheckout(checkout);
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
            var checkouts = await _checkoutRepository.GetSalespersonCheckouts(userId);
            return Ok(checkouts);
        }

        [HttpGet("credential")]
        public async Task<ActionResult<SalespersonCredential>> GetCredential()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var credential = await _credentialRepository.GetSalespersonCredetial(userId);
            return Ok(credential);
        }

        [HttpPut("credential")]
        public async Task<ActionResult<SalespersonCredential>> UpdateOrCreateCredential(SalespersonCredential salespersonCredential)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var credential = await _credentialRepository.GetSalespersonCredetial(userId);
            if (credential == null)
            {
                salespersonCredential.UserId = userId;
                await _credentialRepository.CreateCredential(salespersonCredential);
                user.SalespersonCredentialId = salespersonCredential.Id;
                user.CredentialAccepted = false;
            }
            else
            {
                salespersonCredential.UserId = userId;
                _credentialRepository.UpdateCredetial(salespersonCredential);
            }

            await _userManager.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return Ok(credential);
        }

        [HttpGet("credential/Accepted")]
        public async Task<ActionResult<bool>> CheckSalespersonCredentialAccepted()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user.CredentialAccepted == true;
        }

    }
}