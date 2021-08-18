using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly StoreContext _context;
        public ProgressRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfAlreadyListenedAsync(int courseId, string userId)
        {
            return await _context.Progresses.AnyAsync(p => p.CourseId == courseId && p.UserId == userId);
        }

        public async Task<Progress> CreateProgress(Progress progress)
        {
            await _context.Progresses.AddAsync(progress);
            return progress;
        }

        public async Task DeleteProgress(int progressId)
        {
            var progress = await _context.Progresses.FirstOrDefaultAsync(p => p.Id == progressId);
            _context.Progresses.Remove(progress);
        }

        public async Task<Progress> GetCourseProgress(string userId, int courseId)
        {
            return await _context.Progresses.FirstOrDefaultAsync(p => p.UserId == userId && p.CourseId == courseId);
        }

        public void UpdateProgress(Progress progress)
        {
            _context.Progresses.Update(progress);
        }
    }
}