using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ISMSService
    {
        bool SendSMS(string receptor, string authToken);
        string GenerateAuthToken();
        Task<User> FindUserByPhoneNumberAsync(string phoneNumber);
    }
}