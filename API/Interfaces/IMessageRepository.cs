using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models.Messages;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetGeneralMessagesAsync(bool withDateLimit = true);
        Task<IEnumerable<MessageDto>> SetUserIsSeenForGeneralMessagesAsync(string userId, IEnumerable<MessageDto> messageDtos);
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<MessageDto>> GetUserMessagesAsync(string userId, bool onlyUnseen = false, bool withDateLimit = true);
        Task<Message> CreateMessageAsync(Message message);
        Message EditMessage(Message message);
        Task DeleteMessage(int messageId);
        Task SetUserMessagesStatus(MessagesSetStatusDto setStatusDto);
    }
}