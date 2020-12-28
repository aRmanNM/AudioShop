using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly StoreContext _context;
        public AdminRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourse(Course course)
        {
            var newCourse = await _context.Courses.AddAsync(course);
            await SaveChanges();
            return newCourse.Entity;
        }

        public async Task<CourseEpisode> CreateEpisode(CourseEpisode courseEpisode)
        {
            var newCourseEpisode = await _context.CourseEpisode.AddAsync(courseEpisode);
            await SaveChanges();
            return newCourseEpisode.Entity;
        }

        public async Task DeleteCourses(IEnumerable<Course> courses)
        {
            _context.Courses.RemoveRange(courses);
            await SaveChanges();
        }

        public async Task DeleteEpisodes(IEnumerable<CourseEpisode> courseEpisodes)
        {
            _context.CourseEpisode.RemoveRange(courseEpisodes);
            await SaveChanges();
        }


        public async Task<Course> UpdateCourse(Course course)
        {
            var updatedCourse = _context.Courses.Update(course);
            await SaveChanges();
            return updatedCourse.Entity;
        }

        public async Task<CourseEpisode> UpdateEpisode(CourseEpisode courseEpisode)
        {
            var updatedCourseEpisode = _context.CourseEpisode.Update(courseEpisode);
            await SaveChanges();
            return updatedCourseEpisode.Entity;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}