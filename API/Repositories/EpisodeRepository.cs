using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly StoreContext _context;

        public EpisodeRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Episode> CreateEpisode(Episode courseEpisode)
        {
            var newCourseEpisode = await _context.Episodes.AddAsync(courseEpisode);
            return newCourseEpisode.Entity;
        }

        public Episode UpdateEpisode(Episode courseEpisode)
        {
            var updatedCourseEpisode = _context.Episodes.Update(courseEpisode);
            return updatedCourseEpisode.Entity;
        }

        public void DeleteEpisodes(IEnumerable<Episode> courseEpisodes)
        {
            _context.Episodes.RemoveRange(courseEpisodes);
        }

        public async Task<IEnumerable<Episode>> GetCourseEpisodes(int courseId)
        {
            return await _context.Episodes.Where(x => x.CourseId == courseId)
                .Include(x => x.Course).ToListAsync();
        }

        public async Task<Episode> GetEpisodeById(int id)
        {
            return await _context.Episodes.Include(x => x.Audios).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Episode>> GetUserEpisodes(string userId)
        {
            return await _context.Orders
                .Include(o => o.Episodes)
                .Where(o => o.UserId == userId && o.Status == true)
                .SelectMany(o => o.Episodes)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserEpisodeIds(string userId)
        {
           return await _context.Orders
                .Include(o => o.Episodes)
                .Where(o => o.UserId == userId && o.Status == true)
                .SelectMany(o => o.Episodes).Select(e => e.Id).ToListAsync();
        }
    }
}
