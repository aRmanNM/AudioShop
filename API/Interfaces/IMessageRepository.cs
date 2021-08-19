using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models.Messages;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetGeneralMessagesAsync();
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<MessageDto>> GetUserMessagesAsync(string userId, bool onlyUnseen = false);
        Task<Message> CreateMessageAsync(Message message);
        Message EditMessage(Message message);
        Task DeleteMessage(int messageId);
        Task SetUserMessageToSeen(string userId, int messageId);
    }
}