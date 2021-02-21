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

        public SalespersonController(UserManager<User> userManager,
            IOrderRepository orderRepository,
            IMapperService mapper,
            ICheckoutRepository checkoutRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _checkoutRepository = checkoutRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderForSalespersonDto>>> GetOrdersForCheckout()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var orders = await _orderRepository.GetOrdersForCheckout(user.CouponCode);
            var ordersForSalesperson =
                orders.Select(o => _mapper.MapOrderToOrderForSalespersonDto(o, user.SalePercentageOfSalesperson));
            return Ok(ordersForSalesperson);
        }

        [HttpPost("checkouts")]
        public async Task<ActionResult<Checkout>> CreateCheckout()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
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
            var checkouts = await _checkoutRepository.GetCheckouts(userId);
            return Ok(checkouts);
        }
    }
}