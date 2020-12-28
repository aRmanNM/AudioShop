using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {

        /// <summary>Returns all courses user purchased
        /// </summary>
         Task<IEnumerable<Course>> GetUserCourses(string userId);
        /// <summary>Returns IDs of all courses user purchased
        /// </summary>
         Task<IEnumerable<int>> GetUserCourseIds(string userId);
    }
}