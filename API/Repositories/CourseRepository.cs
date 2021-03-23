using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
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

        public async Task<Course> CreateCourseAsync(Course course)
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

        public async Task<PaginatedResult<Course>> GetCoursesAsync(bool includeEpisodes,
            string search, bool includeInactive = false, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > 20 || pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var courses = _context.Courses.Include(c => c.Photo).AsQueryable();

            if (includeEpisodes)
            {
                courses = courses.Include(c => c.Episodes);
            }

            if (!string.IsNullOrEmpty(search))
            {
                courses = courses.Where(c => c.Name.Contains(search));
            }

            if (!includeInactive)
            {
                courses = courses.Where(c => c.IsActive);
            }

            var result = new PaginatedResult<Course>();
            result.TotalItems = await courses.CountAsync();
            result.Items = await courses.OrderByDescending(c => c.Id)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToArrayAsync();

            return result;
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Episodes)
                .Include(c => c.Photo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
