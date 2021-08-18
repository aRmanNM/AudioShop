using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISMSService
    {
        bool SendVerificationSMS(string receptor, string authToken);
        bool SendMessageSMS(string receptor, string message);
        string GenerateAuthToken();
    }
}