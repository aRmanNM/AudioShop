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
            await _context.Messages.AddAsync(message);
            return message;
        }

        public async Task SetUserMessagesStatus(MessagesSetStatusDto setStatusDto)
        {
            if (setStatusDto.MessageIdsForInAppStatus.Any())
            {
                foreach (var id in setStatusDto.MessageIdsForInAppStatus)
                {
                    var userMessage = await _context.UserMessages.FirstOrDefaultAsync(um => um.MessageId == id && um.UserId == setStatusDto.UserId);
                    if (userMessage == null)
                    {
                        await _context.UserMessages.AddAsync(new UserMessage
                        {
                            MessageId = id,
                            UserId = setStatusDto.UserId,
                            InAppSeen = true
                        });
                    }
                    else
                    {
                        userMessage.InAppSeen = true;
                    }
                }
            }

            if (setStatusDto.MessageIdsForPushStatus.Any())
            {
                foreach (var id in setStatusDto.MessageIdsForPushStatus)
                {
                    var userMessage = await _context.UserMessages.FirstOrDefaultAsync(um => um.MessageId == id && um.UserId == setStatusDto.UserId);
                    if (userMessage == null)
                    {
                        await _context.UserMessages.AddAsync(new UserMessage
                        {
                            MessageId = id,
                            UserId = setStatusDto.UserId,
                            PushSent = true
                        });
                    }
                    else
                    {
                        userMessage.PushSent = true;
                    }
                }
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

        public async Task<IEnumerable<Message>> GetGeneralMessagesAsync(bool withDateLimit = true)
        {
            var messages = _context.Messages
                .Where(m => m.MessageType == MessageType.General ||
                            m.MessageType == MessageType.FreeEpisode ||
                            m.MessageType == MessageType.BuyCourse ||
                            m.MessageType == MessageType.Coupon).AsQueryable();

            if (withDateLimit)
            {
                messages = messages.Where(m => m.CreatedAt > DateTime.Now.AddDays(-20));
            }

            return await messages.AsNoTracking().ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MessageDto>> GetUserMessagesAsync(string userId, bool onlyUnseen = false, bool withDateLimit = true)
        {
            var userMessages = _context.UserMessages
                .Include(um => um.Message)
                .Where(um => um.UserId == userId && um.Message.MessageType == MessageType.User)
                .AsQueryable();

            if (onlyUnseen)
            {
                userMessages = userMessages.Where(um => !um.InAppSeen);
            }

            if (withDateLimit)
            {
                userMessages = userMessages.Where(um => um.Message.CreatedAt > DateTime.Now.AddDays(-20)); // TODO: AddConfig
            }

            return await userMessages.Select(um => new MessageDto
            {
                Id = um.Message.Id,
                Title = um.Message.Title,
                Body = um.Message.Body,
                Link = um.Message.Link,
                UserId = um.Message.UserId,
                CourseId = um.Message.CourseId,
                RepeatAfterHour = um.Message.RepeatAfterHour,
                CreatedAt = um.Message.CreatedAt,
                IsRepeatable = um.Message.IsRepeatable,
                InAppSeen = um.InAppSeen,
                PushSent = um.PushSent,
                SMSSent = um.SMSSent,
                MessageType = um.Message.MessageType,
                SendPush = um.Message.SendPush,
                SendSMS = um.Message.SendSMS,
                SendInApp = um.Message.SendInApp
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<MessageDto>> SetUserIsSeenForGeneralMessagesAsync(string userId, IEnumerable<MessageDto> messageDtos)
        {
            // this method recieves all general messages
            // and updates their flags based on userMessages table values,
            // then returns them
            var messageIds = messageDtos.Select(m => m.Id).ToList();
            var userMessages = await _context.UserMessages
                .Include(um => um.Message)
                .Where(um => messageIds.Any(mi => mi == um.MessageId) && um.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            if (!userMessages.Any())
            {
                return messageDtos;
            }

            bool resetProps;
            MessageDto message;
            foreach (var item in userMessages)
            {
                message = messageDtos.FirstOrDefault(m => m.Id == item.MessageId);
                resetProps = false;

                if (message.IsRepeatable)
                {
                    // logic for repeatable messages:
                    // calculating days and hours of repeatAfterHour property
                    // and comparing with today's info and message creation date
                    if ((DateTime.Now.DayOfYear - item.Message.CreatedAt.DayOfYear) % (item.Message.RepeatAfterHour / 24) == 0 &&
                        item.Message.CreatedAt.AddHours(item.Message.RepeatAfterHour % 24).Hour == DateTime.Now.Hour)
                    {
                        resetProps = true;
                    }
                }

                if (resetProps)
                {
                    message.InAppSeen = item.InAppSeen; // in app messages is not populated by job
                    message.PushSent = false;
                }
                else
                {
                    message.InAppSeen = item.InAppSeen;
                    message.PushSent = item.PushSent;
                }
            }

            return messageDtos;
        }
    }
}