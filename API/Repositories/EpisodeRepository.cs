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

        public async Task<Episode> CreateEpisodeAsync(Episode courseEpisode)
        {
            await _context.Episodes.AddAsync(courseEpisode);
            return courseEpisode;
        }

        public Episode UpdateEpisode(Episode episode)
        {
            // _context.Episodes.Update(courseEpisode);
            _context.Entry(episode).State = EntityState.Modified;
            _context.Entry(episode).Property(e => e.TotalAudiosDuration).IsModified = false;
            return episode;
        }

        public void DeleteEpisode(Episode episode)
        {
            _context.Episodes.Remove(episode);
        }

        public async Task<IEnumerable<Episode>> GetCourseEpisodesAsync(int courseId)
        {
            return await _context.Episodes.Where(x => x.CourseId == courseId)
                .Include(x => x.Course)
                .OrderBy(e => e.Sort)
                .ToListAsync();
        }

        public async Task<Episode> GetEpisodeByIdAsync(int id)
        {
            return await _context.Episodes.Include(x => x.Audios).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Episode>> GetUserEpisodesAsync(string userId)
        {
            return await _context.OrderEpisodes
                .Include(o => o.Order)
                .Include(e => e.Episode)
                .Where(oe => oe.Order.Status == true && oe.Order.UserId == userId)
                .Select(ep => ep.Episode)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserEpisodeIdsAsync(string userId)
        {
            return await _context.OrderEpisodes
                .Include(o => o.Order)
                .Include(e => e.Episode)
                .Where(oe => oe.Order.Status == true && oe.Order.UserId == userId)
                .Select(ep => ep.Episode.Id)
                .ToListAsync();
        }

        public void UpdateEpisodes(Episode[] episodes)
        {
            _context.Episodes.UpdateRange(episodes);
        }

        public async Task<bool> CheckIfAlreadyBoughtAsync(Episode episode)
        {
            return await _context.OrderEpisodes
                .Include(oe => oe.Order)
                .FirstOrDefaultAsync(oe => oe.EpisodeId == episode.Id && oe.Order.Status == true) != null;

        }
    }
}
