using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;

        public AdminController(UserManager<User> userManager,
            ICheckoutRepository checkoutRepository,
            IMapperService mapper,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment host)
        {
            _userManager = userManager;
            _checkoutRepository = checkoutRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _host = host;
        }

        //
        // MEMBERS
        //

        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<User>>> GetMembers()
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            return Ok(members);
        }

        [HttpGet("members/{id}")]
        public async Task<ActionResult<User>> UpdateMember(User member)
        {
            var res = await _userManager.UpdateAsync(member);
            if (!res.Succeeded)
            {
                return BadRequest("failed to edit user");
            }

            return Ok(member);
        }

        //
        // CHECKOUTS
        //

        [HttpGet("checkouts")]
        public async Task<ActionResult<IEnumerable<Checkout>>> GetCheckouts(bool status)
        {
            var checkouts = await _checkoutRepository.GetCheckouts(status);
            return Ok(checkouts);
        }

        [HttpPut("checkouts/{id}")]
        public async Task<ActionResult<Checkout>> EditCheckout(Checkout checkout)
        {
            checkout.PaidAt = DateTime.Now;
            _checkoutRepository.EditCheckout(checkout);
            return Ok(checkout);
        }
    }
}