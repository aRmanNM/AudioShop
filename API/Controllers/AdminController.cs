using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IMapperService _mapper;
        private readonly IUserRepository _userRepository;        

        public AdminController(UserManager<User> userManager,
            ICheckoutRepository checkoutRepository,
            IUnitOfWork unitOfWork,            
            IMapperService mapper,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _checkoutRepository = checkoutRepository;
            _unitOfWork = unitOfWork;            
            _mapper = mapper;
            _userRepository = userRepository;
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
        public async Task<ActionResult<IEnumerable<Checkout>>> GetCheckouts(bool status, string userName = null, bool includeSalespersonInfo = false)
        {
            var checkouts = await _checkoutRepository.GetCheckouts(status, userName, includeSalespersonInfo);
            return Ok(checkouts);
        }

        [HttpGet("checkouts/{checkoutId}")]
        public async Task<ActionResult<Checkout>> GetCheckoutWithInfo(int checkoutId)
        {
            var checkout = await _checkoutRepository.GetCheckoutWithId(checkoutId);
            if (checkout == null)
            {
                return NotFound("cehckout not found");
            }

            return Ok(checkout);
        }

        [HttpPut("checkouts/{id}")]
        public async Task<ActionResult<Checkout>> EditCheckout(Checkout checkout)
        {
            checkout.PaidAt = DateTime.Now;
            _checkoutRepository.EditCheckout(checkout);
            await _unitOfWork.CompleteAsync();
            return Ok(checkout);
        }

        [HttpGet("salespersons")]
        public async Task<ActionResult<PaginatedResult<SalespersonDto>>> GetAllSalespersons(string search = null,
            bool onlyShowUsersWithUnacceptedCred = false, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _userRepository.GetAllSalespersons(search, onlyShowUsersWithUnacceptedCred, pageNumber, pageSize);
            var resultWithDtos = new PaginatedResult<SalespersonDto>();
            resultWithDtos.TotalItems = result.TotalItems;
            resultWithDtos.Items = result.Items.Select(s => _mapper.MapUserToSalespersonDto(s));
            return Ok(resultWithDtos);
        }

        [HttpGet("salespersons/{userId}")]
        public async Task<ActionResult<SalespersonDto>> GetSalesperson(string userId)
        {
            var salesperson = await _userRepository.FindUserById(userId);
            return Ok(_mapper.MapUserToSalespersonDto(salesperson));
        }

        [HttpPut("salespersons/{userId}")]
        public async Task<ActionResult<SalespersonDto>> UpdateCredential(string userId)
        {
            var salesperson = await _userManager.FindByIdAsync(userId);
            salesperson.CredentialAccepted = !salesperson.CredentialAccepted;
            await _userManager.UpdateAsync(salesperson);
            return Ok();
        }      
    }
}