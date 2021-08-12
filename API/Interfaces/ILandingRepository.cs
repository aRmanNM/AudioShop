using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models.Landing;

namespace API.Interfaces
{
    public interface ILandingRepository
    {
        Task<IEnumerable<LandingDto>> GetLandings();
        Task<LandingDto> GetLandingDtoById(int landingId);
        Task<Landing> GetLandingById(int landingId);
        Task<Landing> CreateLanding(Landing landing);
        Landing EditLanding(Landing landing);
        Task CreateLandingPhoneNumber(LandingPhoneNumber landingPhoneNumber);
        Task<IEnumerable<LandingPhoneNumber>> GetLandingPhoneNumbers(int landingId);
        Task UpdateLandingStat(int landingId);
    }
}