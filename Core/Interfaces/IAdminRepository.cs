using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<Course> CreateCourse(Course course);
        Task<Course> UpdateCourse(Course course);
        Task DeleteCourses(IEnumerable<Course> IDs);
        Task<CourseEpisode> CreateEpisode(CourseEpisode courseEpisode);
        Task<CourseEpisode> UpdateEpisode(CourseEpisode courseEpisode);
        Task DeleteEpisodes(IEnumerable<CourseEpisode> IDs);
        Task SaveChanges();

    }
}