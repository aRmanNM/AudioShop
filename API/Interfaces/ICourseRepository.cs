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
        Task<Course> CreateCourse(Course course);
        Course UpdateCourse(Course course);
        void DeleteCourses(IEnumerable<Course> IDs); // TODO : THIS SEEMS WRONG
        Task<PaginatedResult<Course>> GetCourses(bool includeEpisodes,
            string search, bool includeInactive = false, int pageNumber = 1, int pageSize = 10);
        Task<Course> GetCourseById(int id);
    }
}
