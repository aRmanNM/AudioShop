using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.Landing;

namespace API.Interfaces
{
    public interface ILandingRepository
    {
        Task<IEnumerable<Landing>> GetLandings();
        Task<Landing> GetLandingById(int landingId);
        Task<Landing> CreateLanding(Landing landing);
        Landing EditLanding(Landing landing);
        Task CreateLandingPhoneNumber(LandingPhoneNumber landingPhoneNumber);
        Task<IEnumerable<LandingPhoneNumber>> GetLandingPhoneNumbers(int landingId);
    }
}