using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourse(Course course);
        Course UpdateCourse(Course course);
        void DeleteCourses(IEnumerable<Course> IDs); // TODO : THIS SEEMS WRONG
        Task<IEnumerable<Course>> GetCourses(bool includeEpisodes);
        Task<Course> GetCourseById(int id);
    }
}
