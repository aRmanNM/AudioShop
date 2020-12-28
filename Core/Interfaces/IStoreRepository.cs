using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Course>> GetCourses();

        Task<Course> GetCourseById(int id);

        Task<IEnumerable<CourseEpisode>> GetCourseEpisodes(int courseId);

        Task<CourseEpisode> GetCourseEpisodeById(int id);
    }
}