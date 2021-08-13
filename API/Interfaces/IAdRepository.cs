using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.Ads;

namespace API.Interfaces
{
    public interface IAdRepository
    {
        Task<IEnumerable<Ad>> GetAdsAsync();
        Task<Ad> GetAdByIdAsync(int adId);
        Task<IEnumerable<Ad>> GetAdsByTitleEnAsync(string titleEn);
        Task<Ad> CreateAdAsync(Ad ad);
        void EditAd(Ad ad);
        Task<IEnumerable<Place>> GetPlacesAsync();
        Task<IEnumerable<Place>> GetPlacesByIdsAsync(List<int> ids);
        Place EditPlace(Place place);
        Task DeleteAdPlaces(int adId);
        Task<IEnumerable<AdPlace>> AdddAdPlaces(ICollection<AdPlace> adPlaces);
    }
}