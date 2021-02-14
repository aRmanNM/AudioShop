﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
            await _context.Episodes.AddAsync(courseEpisode);
            return courseEpisode;
        }

        public Episode UpdateEpisode(Episode courseEpisode)
        {
            _context.Episodes.Update(courseEpisode);
            return courseEpisode;
        }

        public void DeleteEpisode(Episode episode)
        {
            _context.Episodes.Remove(episode);
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
            return await _context.OrderEpisodes
                .Include(o => o.Order)
                .Include(e => e.Episode)
                .Where(oe => oe.Order.Status == true && oe.Order.UserId == userId)
                .Select(ep => ep.Episode)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserEpisodeIds(string userId)
        {
            return await _context.OrderEpisodes
                .Include(o => o.Order)
                .Include(e => e.Episode)
                .Where(oe => oe.Order.Status == true && oe.Order.UserId == userId)
                .Select(ep => ep.Episode.Id)
                .ToListAsync();
        }
    }
}
