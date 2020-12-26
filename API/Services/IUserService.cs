using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services
{
    public interface IUserService
    {

        /// <summary>Returns all courses user purchased
        /// </summary>
         Task<IEnumerable<Course>> GetUserCourses(string userId);
        /// <summary>Returns IDs of all courses user purchased
        /// </summary>
         Task<IEnumerable<int>> GetUserCourseIds(string userId);
    }
}