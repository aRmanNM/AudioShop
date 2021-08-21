using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Models.Messages;
using API.Models.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISMSService _smsService;
        private readonly UserManager<User> _userManager;
        private readonly IMessageRepository _messageRepository;

        public TicketsController(ITicketRepository ticketRepository,
            IUnitOfWork unitOfWork,
            ISMSService smsService,
            UserManager<User> userManager,
            IMessageRepository messageRepository)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _userManager = userManager;
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            await _ticketRepository.CreateTicket(ticket);
            await _unitOfWork.CompleteAsync();
            return Ok(ticket);
        }

        [HttpPost("response")]
        public async Task<ActionResult<TicketResponse>> CreateTicketResponse(TicketResponse ticketResponse)
        {
            await _ticketRepository.CreateResponse(ticketResponse);

            var ticket = await _ticketRepository.GetTicketById(ticketResponse.TicketId);
            if (ticketResponse.IssuedByAdmin)
            {
                var user = await _userManager.FindByIdAsync(ticket.UserId);
                if (user == null)
                {
                    return BadRequest();
                }

                string messageBody = "تیکت با عنوان «" + ticket.Title + "» از طرف ادمین پاسخ داده شد";
                var message = new Message
                {
                    Title = "پاسخ تیکت",
                    Body = messageBody,
                    ClockRangeBegin = ticket.CreatedAt.AddMinutes(5).Hour,
                    ClockRangeEnd = ticket.CreatedAt.AddHours(1).Hour,
                    MessageType = MessageType.User,
                    IsRepeatable = false,
                    Link = ticket.Id.ToString(),
                    SendPush = true,
                    SendSMS = true,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    CourseId = 0,
                };

                await _messageRepository.CreateMessageAsync(message);

                ticket.TicketStatus = TicketStatus.AdminAnswered;

                if (message.SendSMS && user.PhoneNumberConfirmed)
                {
                    _smsService.SendMessageSMS(user.PhoneNumber, message.Body);
                }
            }
            else
            {
                ticket.TicketStatus = TicketStatus.MemberAnswered;
            }

            await _unitOfWork.CompleteAsync();
            return Ok(ticketResponse);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Ticket>> GetTickets(bool isFinished = false)
        {
            var tickets = await _ticketRepository.GetTickets(isFinished);
            if (!tickets.Any())
            {
                return NoContent();
            }

            return Ok(tickets);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetUserTickets(string userId)
        {
            var tickets = await _ticketRepository.GetUserTickets(userId);
            if (!tickets.Any())
            {
                return NoContent();
            }

            return Ok(tickets);
        }

        [HttpGet("{ticketId}")]
        public async Task<ActionResult<Ticket>> GetTicketWithResponses(int ticketId)
        {
            var tickets = await _ticketRepository.GetTicketWithResponsesById(ticketId);
            if (tickets == null)
            {
                return NoContent();
            }

            return Ok(tickets);
        }

        [HttpPut("{ticketId}")]
        public async Task<ActionResult<Ticket>> FinishTicket(int ticketId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ticket = await _ticketRepository.GetTicketById(ticketId);

            if (userId == null || ticket.UserId != userId)
            {
                return BadRequest();
            }

            ticket.TicketStatus = TicketStatus.Finished;
            _ticketRepository.EditTicket(ticket);
            await _unitOfWork.CompleteAsync();

            return Ok(ticket);
        }
    }
}