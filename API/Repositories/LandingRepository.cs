using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
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

        public async Task<LandingDto> GetLandingDtoById(int landingId)
        {
            return await _context.Landings
                .Include(l => l.PhoneNumbers)
                .Select(l => new LandingDto
                {
                    Id = l.Id,
                    Description = l.Description,
                    Button = l.Button,
                    ButtonClickCount = l.ButtonClickCount,
                    ButtonEnabled = l.ButtonEnabled,
                    ButtonLink = l.ButtonLink,
                    GiftEnabled = l.GiftEnabled,
                    Gift = l.Gift,
                    Logo = l.Logo,
                    LogoEnabled = l.LogoEnabled,
                    Media = l.Media,
                    MediaEnabled = l.MediaEnabled,
                    PhoneBoxEnabled = l.PhoneBoxEnabled,
                    Text1 = l.Text1,
                    Text1Enabled = l.Text1Enabled,
                    Text2 = l.Text2,
                    Text2Enabled = l.Text2Enabled,
                    Title = l.Title,
                    TitleEnabled = l.TitleEnabled,
                    ButtonsColor = l.ButtonsColor,
                    Background = l.Background,
                    PhoneNumberCounts = l.PhoneNumbers.Count(),
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == landingId);
        }

        public async Task<Landing> GetLandingById(int landingId)
        {

            return await _context.Landings
               .Include(c => c.Logo)
               .Include(c => c.Media)
               .Include(c => c.Background)
               .FirstOrDefaultAsync(c => c.Id == landingId);

        }

        public async Task<IEnumerable<LandingPhoneNumber>> GetLandingPhoneNumbers(int landingId)
        {
            return await _context.LandingPhoneNumbers.Where(pn => pn.LandingId == landingId).ToListAsync();
        }

        public async Task<IEnumerable<LandingDto>> GetLandings()
        {
            return await _context.Landings
                .Include(l => l.PhoneNumbers)
                .Select(l => new LandingDto
                {
                    Id = l.Id,
                    Description = l.Description,
                    Button = l.Button,
                    ButtonClickCount = l.ButtonClickCount,
                    ButtonEnabled = l.ButtonEnabled,
                    ButtonLink = l.ButtonLink,
                    GiftEnabled = l.GiftEnabled,
                    Gift = l.Gift,
                    Logo = l.Logo,
                    LogoEnabled = l.LogoEnabled,
                    Media = l.Media,
                    MediaEnabled = l.MediaEnabled,
                    PhoneBoxEnabled = l.PhoneBoxEnabled,
                    Text1 = l.Text1,
                    Text1Enabled = l.Text1Enabled,
                    Text2 = l.Text2,
                    Text2Enabled = l.Text2Enabled,
                    Title = l.Title,
                    TitleEnabled = l.TitleEnabled,
                    Background = l.Background,
                    ButtonsColor = l.ButtonsColor,
                    PhoneNumberCounts = l.PhoneNumbers.Count()
                }).ToListAsync();
        }

        public async Task UpdateLandingStat(int landingId)
        {
            var landing = await _context.Landings.FirstOrDefaultAsync(l => l.Id == landingId);
            landing.ButtonClickCount ++;
        }
    }
}