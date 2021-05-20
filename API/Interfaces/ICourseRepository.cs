using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourseAsync(Course course);
        Course UpdateCourse(Course course);
        Task<PaginatedResult<Course>> GetCoursesAsync(bool includeEpisodes,
            string search, bool includeInactive = false, int pageNumber = 1, int pageSize = 10, string category = null);
        Task<Course> GetCourseByIdAsync(int id, bool withTracking = false);
    }
}
