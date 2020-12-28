using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly StoreContext _context;
        public StoreRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<IEnumerable<CourseEpisode>> GetCourseEpisodes(int courseId)
        {
            return await _context.CourseEpisode.Where(x => x.CourseId == courseId)
                .Include(x => x.Course).ToListAsync();
        }

        public async Task<CourseEpisode> GetCourseEpisodeById(int id)
        {
            return await _context.CourseEpisode.Include(x => x.Course).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}