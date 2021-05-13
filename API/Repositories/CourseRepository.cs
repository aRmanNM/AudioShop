using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var courses = _context.Courses.AsQueryable();

            // if (includeEpisodes)
            // {
            //     courses = courses.Include(c => c.Episodes);
            // }

            if (!string.IsNullOrEmpty(search))
            {
                courses = courses.Where(c => c.Name.Contains(search) || c.Instructor.Contains(search) || c.Description.Contains(search));
            }

            if (!includeInactive)
            {
                courses = courses.Where(c => c.IsActive);
            }

            var emptyCollection = new Collection<Episode>();

            var result = new PaginatedResult<Course>();
            result.TotalItems = await courses.CountAsync();
            result.Items = await courses.OrderByDescending(c => c.LastEdited)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    Description = c.Description,
                    WaitingTimeBetweenEpisodes = c.WaitingTimeBetweenEpisodes,
                    IsActive = c.IsActive,
                    Photo = c.Photo,
                    Episodes =  includeEpisodes ? c.Episodes : emptyCollection,
                    Reviews = c.Reviews,
                    Instructor = c.Instructor,
                    AverageScore = (short)c.Reviews.Select(r => (double?)r.Rating).Average()
                })
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToArrayAsync();

            return result;
        }

        public async Task<Course> GetCourseByIdAsync(int id, bool withTracking = false)
        {
            if (withTracking)
            {
                return await _context.Courses
               .Include(c => c.Episodes)
               .Include(c => c.Photo)
               .Include(c => c.Reviews)
               .FirstOrDefaultAsync(c => c.Id == id);
            }
            else
            {
                return await _context.Courses.Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    Description = c.Description,
                    WaitingTimeBetweenEpisodes = c.WaitingTimeBetweenEpisodes,
                    IsActive = c.IsActive,
                    Photo = c.Photo,
                    Episodes = c.Episodes,
                    Reviews = c.Reviews,
                    Instructor = c.Instructor,
                    AverageScore = (short)c.Reviews.Select(r => (double?)r.Rating).Average()
                }).FirstOrDefaultAsync(c => c.Id == id);
            }
        }
    }
}
