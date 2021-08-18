using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models.Messages;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StoreContext _context;

        public MessageRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            await _context.Messages.AddRangeAsync(message);

            if (message.MessageType == MessageType.User)
            {
               await _context.UserMessages.AddAsync(new UserMessage {
                    MessageId = message.Id,
                    UserId = message.UserId,
                    IsSeen = true
                });
            }

            return message;
        }

        public async Task SetUserMessageToSeen(string userId, int messageId)
        {
            var userMessage = await _context.UserMessages.FirstOrDefaultAsync(um => um.MessageId == messageId && um.UserId == userId);
            if (userMessage == null)
            {
                await _context.UserMessages.AddAsync(new UserMessage {
                    MessageId = messageId,
                    UserId = userId,
                    IsSeen = true
                });
            }

            userMessage.IsSeen = true;
        }

        public Message DeleteMessage(int messageId)
        {
            throw new System.NotImplementedException();
        }

        public Message EditMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetGeneralMessagesAsync()
        {
            return await _context.Messages
                .Where(m => m.MessageType == MessageType.General ||
                            m.MessageType == MessageType.FreeEpisode ||
                            m.MessageType == MessageType.BuyCourse).ToListAsync();
        }

        public Task<Message> GetMessageByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<MessageDto>> GetUserMessagesAsync(string userId, bool onlyUnseen = false)
        {
            var userMessages = _context.UserMessages.AsQueryable();

            if (onlyUnseen)
            {
                userMessages = userMessages.Where(um => !um.IsSeen);
            }

            return await userMessages.Select(um => new MessageDto
            {
                Id = um.Message.Id,
                Title = um.Message.Title,
                Body = um.Message.Body,
                Link = um.Message.Link,
                UserId = um.Message.UserId,
                CourseId = um.Message.CourseId,
                ClockRangeBegin = um.Message.ClockRangeBegin,
                ClockRangeEnd = um.Message.ClockRangeEnd,
                CreatedAt = um.Message.CreatedAt,
                IsRepeatable = um.Message.IsRepeatable,
                IsSeen = um.IsSeen,
                MessageType = um.Message.MessageType,
                SendPush = um.Message.SendPush,
                SendSMS = um.Message.SendSMS
            }).ToListAsync();
        }
    }
}