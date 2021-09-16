using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapperService _mapperService;
        private readonly ICourseRepository _courseRepository;
        private readonly IProgressRepository _progressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISMSService _smsService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageRepository messageRepository,
            IMapperService mapperService,
            ICourseRepository courseRepository,
            IProgressRepository progressRepository,
            IUnitOfWork unitOfWork,
            ISMSService smsService,
            IUserRepository userRepository,
            ILogger<MessagesController> logger)
        {
            _courseRepository = courseRepository;
            _progressRepository = progressRepository;
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _userRepository = userRepository;
            _logger = logger;
            _messageRepository = messageRepository;
            _mapperService = mapperService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetGeneralMessages()
        {
            var messages = await _messageRepository.GetGeneralMessagesAsync();
            var messageDtos = messages.Select(m => _mapperService.MapMessageToMessageDto(m));
            return Ok(messageDtos);
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(string userId, [FromQuery] bool onlyUnseen = false, [FromQuery] bool onlyUserMessages = false, [FromQuery] bool withDateLimit = true)
        {
            // A hack to enable entering userName :|
            Guid something;
            if (!Guid.TryParse(userId, out something))
            {
                var user = await _userRepository.FindUserByUserNameAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                userId = user.Id;
            }


            //
            // TODO: Following codes needs serious refactoring!
            //

            var messages = new List<MessageDto>();

            var userMessageDtos = (await _messageRepository.GetUserMessagesAsync(userId, onlyUnseen, withDateLimit)).ToList();

            messages.AddRange(userMessageDtos);

            if (!onlyUserMessages)
            {
                var generalMessages = await _messageRepository.GetGeneralMessagesAsync(withDateLimit);
                var generalMessageDtos = generalMessages.Select(gm => _mapperService.MapMessageToMessageDto(gm)).ToList();
                generalMessageDtos = (await _messageRepository.SetUserIsSeenForGeneralMessagesAsync(userId, generalMessageDtos)).ToList();

                if (onlyUnseen)
                {
                    var filteredGeneralMessageDtos = generalMessageDtos.Where(gmd => !gmd.InAppSeen).ToList();
                    messages.AddRange(filteredGeneralMessageDtos);
                }
                else
                {
                    messages.AddRange(generalMessageDtos);
                }
            }

            messages = messages.OrderByDescending(m => m.CreatedAt).ToList();

            // filter course messages and coupon messages if user already bought atleast one episode of course
            var courseMessages = messages.Where(m => m.MessageType == MessageType.BuyCourse &&
                m.MessageType == MessageType.Coupon).ToList();

            if (courseMessages.Any())
            {
                foreach (var courseMessage in courseMessages)
                {
                    var res = await _courseRepository.CheckIfAlreadyBoughtAsync(courseMessage.CourseId, userId);
                    if (res)
                    {
                        messages.Remove(courseMessage);
                    }
                }
            }

            // filter free episode messages if user already listened atleast one episode of course
            var episodeMessages = messages.Where(m => m.MessageType == MessageType.FreeEpisode).ToList();
            if (episodeMessages.Any())
            {
                foreach (var episodeMessage in episodeMessages)
                {
                    var res = await _progressRepository.CheckIfAlreadyListenedAsync(episodeMessage.CourseId, userId);
                    if (res)
                    {
                        messages.Remove(episodeMessage);
                    }
                }
            }

            // _logger.LogInformation("messages requested for user {userId} at {datetime}", userId, DateTime.Now);
            return Ok(messages);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageDto messageDto)
        {
            if (messageDto.RepeatAfterHour < 24)
            {
                messageDto.RepeatAfterHour = 24;
            }

            if (messageDto.MessageType == MessageType.User || messageDto.SendSMS)
            {
                messageDto.IsRepeatable = false;
            }

            if (messageDto.MessageType != MessageType.User)
            {
                messageDto.SendSMS = false;
            }

            var message = _mapperService.MapMessageDtoToMessage(messageDto);
            await _messageRepository.CreateMessageAsync(message);

            if (message.MessageType == MessageType.User)
            {
                var userMessage = new UserMessage
                {
                    MessageId = message.Id,
                    UserId = message.UserId,
                    PushSent = false,
                    SMSSent = false,
                    InAppSeen = false
                };

                message.UserMessages.Add(userMessage);

                if (message.SendSMS)
                {
                    var user = await _userRepository.FindUserByIdAsync(message.UserId);
                    if (user.PhoneNumberConfirmed)
                    {
                        _smsService.SendMessageSMS(user.PhoneNumber, message.Body);
                    }

                    userMessage.SMSSent = true;
                }
            }

            await _unitOfWork.CompleteAsync();
            return Ok(_mapperService.MapMessageToMessageDto(message));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> EditMessage(MessageDto messageDto)
        {
            var message = _mapperService.MapMessageDtoToMessage(messageDto);
            _messageRepository.EditMessage(message);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            await _messageRepository.DeleteMessage(messageId);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPut("users/{userId}")]
        public async Task<ActionResult> SetMessagesStatus(MessagesSetStatusDto setStatusDto)
        {
            try
            {
                await _messageRepository.SetUserMessagesStatus(setStatusDto);
                await _unitOfWork.CompleteAsync();

                // string statusToSet = "";
                // if (setStatusDto.MessageIdsForInAppStatus.Any())
                // {
                //     statusToSet = "InApp messages status";
                // }
                // else if (setStatusDto.MessageIdsForPushStatus.Any())
                // {
                //     statusToSet = "Push messages status";
                // }
                // else
                // {
                //     statusToSet = "Nothing";
                // }

                // _logger.LogInformation("{status} updated for user {userId} at {datetime}", statusToSet, setStatusDto.UserId, DateTime.Now);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}