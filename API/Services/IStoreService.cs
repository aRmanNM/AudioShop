using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services
{
    public interface IStoreService
    {
        Task<IEnumerable<Course>> GetCourses();

        Task<Course> GetCourseById(int id);

        Task<IEnumerable<CourseEpisode>> GetCourseEpisodes(int courseId);

        Task<CourseEpisode> GetCourseEpisodeById(int id);
    }
}