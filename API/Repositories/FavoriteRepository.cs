using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly StoreContext _context;
        public FavoriteRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Favorite> CreateFavorite(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            return favorite;
        }

        public async Task DeleteFavorite(int favoriteId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.Id == favoriteId);
            _context.Favorites.Remove(favorite);
        }

        public async Task<IEnumerable<Favorite>> GetFavorites(string userId)
        {
            return await _context.Favorites.Where(f => f.UserId == userId).ToArrayAsync();
        }

        public void UpdateFavorite(Favorite favorite)
        {
            _context.Favorites.Update(favorite);
        }
    }
}