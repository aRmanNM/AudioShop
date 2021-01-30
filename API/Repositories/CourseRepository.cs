using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StoreContext _context;

        public CourseRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourse(Course course)
        {
            await _context.Courses.AddAsync(course);
            return course;
        }

        public void DeleteCourses(IEnumerable<Course> courses)
        {
            _context.Courses.RemoveRange(courses);
        }

        public Course UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            return course;
        }

        public async Task<IEnumerable<Course>> GetCourses(bool includeEpisodes)
        {
            if (!includeEpisodes)
            {
                return await _context.Courses
                    .Include(c => c.Photo)
                    .ToListAsync();
            }

            return await _context.Courses
                .Include(c => c.Photo)
                .Include(c => c.Episodes)
                .ToListAsync();

        }
        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses
                .Include(c => c.Episodes)
                .Include(c => c.Photo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
