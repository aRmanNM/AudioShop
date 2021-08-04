using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models.Landing;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class LandingRepository : ILandingRepository
    {
        private readonly StoreContext _context;
        public LandingRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Landing> CreateLanding(Landing landing)
        {
            await _context.Landings.AddAsync(landing);
            return landing;
        }

        public async Task CreateLandingPhoneNumber(LandingPhoneNumber landingPhoneNumber)
        {
            var checkExists = await _context.LandingPhoneNumbers.AnyAsync(pn => pn.PhoneNumber == landingPhoneNumber.PhoneNumber);
            if (checkExists)
            {
                return;
            }

            await _context.LandingPhoneNumbers.AddAsync(landingPhoneNumber);
        }

        public Landing EditLanding(Landing landing)
        {
            _context.Landings.Update(landing);
            return landing;
        }

        public async Task<Landing> GetLandingById(int landingId)
        {
            return await _context.Landings.FirstOrDefaultAsync(l => l.Id == landingId);
        }

        public async Task<IEnumerable<LandingPhoneNumber>> GetLandingPhoneNumbers(int landingId)
        {
            return await _context.LandingPhoneNumbers.Where(pn => pn.LandingId == landingId).ToListAsync();
        }

        public async Task<IEnumerable<Landing>> GetLandings()
        {
            return await _context.Landings.ToListAsync();
        }
    }
}