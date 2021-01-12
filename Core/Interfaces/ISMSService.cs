using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISMSService
    {
        bool SendSMS(string receptor, string authToken);
        string GenerateAuthToken();
    }
}