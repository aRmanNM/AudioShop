using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetFavorites(string userId);
        Task<Favorite> CreateFavorite(Favorite favorite);
        void UpdateFavorite(Favorite favorite);
        Task DeleteFavorite(int favoriteId);
    }
}