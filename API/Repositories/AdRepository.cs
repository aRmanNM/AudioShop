using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models.Ads;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AdRepository : IAdRepository
    {
        private readonly StoreContext _context;
        public AdRepository(StoreContext context)
        {
            _context = context;

        }

        public async Task<Ad> CreateAdAsync(Ad ad)
        {
            await _context.Ads.AddAsync(ad);
            return ad;
        }

        public void EditAd(Ad ad)
        {
            _context.Ads.Update(ad);
        }

        public Place EditPlace(Place place)
        {
            _context.Places.Update(place);
            return place;
        }

        public async Task<Ad> GetAdByIdAsync(int adId)
        {
            return await _context.Ads
                .Include(a => a.File)
                .Include(a => a.AdPlaces).ThenInclude(ap => ap.Place)
                .FirstOrDefaultAsync(a => a.Id == adId);
        }

        public async Task<IEnumerable<Ad>> GetAdsByTitleEnAsync(string titleEn)
        {
            return await _context.AdPlaces
                .Include(ap => ap.Place)
                .Include(ap => ap.Ad)
                .ThenInclude(ap => ap.File)
                .Where(ap => ap.Place.TitleEn.ToLower() == titleEn.ToLower() && ap.Ad.IsEnabled)
                .Select(ap => ap.Ad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ad>> GetAdsAsync()
        {
            return await _context.Ads
                .Include(a => a.File)
                .Include(a => a.AdPlaces).ThenInclude(ap => ap.Place)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetPlacesAsync()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetPlacesByIdsAsync(List<int> ids)
        {
            return await _context.Places.Where(p => ids.Any(i => p.Id == i)).ToListAsync();
        }

        public async Task DeleteAdPlaces(int adId)
        {
            var adPlaces = await _context.AdPlaces.Where(ap => ap.AdId == adId).ToListAsync();
            _context.AdPlaces.RemoveRange(adPlaces);
        }

        public async Task<IEnumerable<AdPlace>> AdddAdPlaces(ICollection<AdPlace> adPlaces)
        {
            await _context.AdPlaces.AddRangeAsync(adPlaces);
            return adPlaces;
        }
    }
}