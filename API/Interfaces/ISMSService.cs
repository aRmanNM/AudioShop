using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISMSService
    {
        bool SendSMS(string receptor, string authToken);
        string GenerateAuthToken();
    }
}