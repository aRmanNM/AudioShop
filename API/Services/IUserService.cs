using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services
{
    public interface IUserService
    {
         Task<IEnumerable<Course>> GetUserCourses(string userId);
    }
}