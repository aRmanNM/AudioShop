using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;

        public AdminController(UserManager<User> userManager,
            ICheckoutRepository checkoutRepository,
            IUnitOfWork unitOfWork,
            IReviewRepository reviewRepository)
        {
            _userManager = userManager;
            _checkoutRepository = checkoutRepository;
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
        }

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

        [HttpGet("checkouts")]
        public async Task<ActionResult<IEnumerable<Checkout>>> GetCheckouts(bool status, string userName = null)
        {
            var checkouts = await _checkoutRepository.GetCheckouts(status, userName);
            return Ok(checkouts);
        }

        [HttpPut("checkouts/{id}")]
        public async Task<ActionResult<Checkout>> EditCheckout(Checkout checkout)
        {
            checkout.PaidAt = DateTime.Now;
            _checkoutRepository.EditCheckout(checkout);
            await _unitOfWork.CompleteAsync();
            return Ok(checkout);
        }

        [HttpGet("courses/{courseId}/reviews")]
        public async Task<ActionResult<List<Review>>> GetAllCourseReviews(int courseId, bool accepted)
        {
            var reviews = await _reviewRepository.GetCourseReviews(courseId, accepted);
            return Ok(reviews);
        }

    }
}