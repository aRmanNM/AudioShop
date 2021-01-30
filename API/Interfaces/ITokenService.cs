using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
