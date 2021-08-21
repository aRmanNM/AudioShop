using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models.Messages;
using Microsoft.AspNetCore.Mvc;
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
                await _context.UserMessages.AddAsync(new UserMessage
                {
                    Message = message,
                    UserId = message.UserId,
                    IsSeen = false
                });
            }

            return message;
        }

        public async Task SetUserMessageToSeen(string userId, int messageId)
        {
            var userMessage = await _context.UserMessages.FirstOrDefaultAsync(um => um.MessageId == messageId && um.UserId == userId);
            if (userMessage == null)
            {
                await _context.UserMessages.AddAsync(new UserMessage
                {
                    MessageId = messageId,
                    UserId = userId,
                    IsSeen = true
                });
            }
            else
            {
                userMessage.IsSeen = true;
            }
        }

        public async Task DeleteMessage(int messageId)
        {
            var message = await GetMessageByIdAsync(messageId);
            _context.Messages.Remove(message);
            var userMessages = await _context.UserMessages.Where(um => um.MessageId == messageId).ToListAsync();
            _context.UserMessages.RemoveRange(userMessages);
        }

        public Message EditMessage(Message message)
        {
            _context.Messages.Update(message);
            return message;
        }

        public async Task<IEnumerable<Message>> GetGeneralMessagesAsync()
        {
            return await _context.Messages
                .Where(m => m.MessageType == MessageType.General ||
                            m.MessageType == MessageType.FreeEpisode ||
                            m.MessageType == MessageType.BuyCourse ||
                            m.MessageType == MessageType.Coupon).ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MessageDto>> GetUserMessagesAsync(string userId, bool onlyUnseen = false)
        {
            var userMessages = _context.UserMessages
                .Include(um => um.Message)
                .Where(um => um.UserId == userId && um.Message.MessageType == MessageType.User)
                .AsQueryable();

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

        public async Task<IEnumerable<MessageDto>> SetUserIsSeenForGeneralMessagesAsync(string userId, IEnumerable<MessageDto> messageDtos)
        {
            var messageIds = messageDtos.Select(m => m.Id).ToList();
            var userMessages = await _context.UserMessages
                .Where(um => messageIds.Any(mi => mi == um.MessageId) && um.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in userMessages)
            {
                messageDtos.FirstOrDefault(m => m.Id == item.MessageId).IsSeen = item.IsSeen;
            }

            return messageDtos;
        }
    }
}