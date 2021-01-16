using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;
        public UserRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetUserCourses(string userId)
        {
            var courses = await (from course in _context.Courses
                                 join basketItems in _context.BasketItems
                                   on course.Id equals basketItems.CourseId
                                 join orders in _context.Orders
                                   on basketItems.OrderId equals orders.Id
                                 where orders.Status == true &&
                                       orders.UserId == userId
                                 select new { course, basketItems, orders }
                                   ).Select(x => x.course).ToListAsync();
            return courses;
        }

        public async Task<IEnumerable<int>> GetUserCourseIds(string userId)
        {
            var courseIds = await (from courses in _context.Courses
                                   join basketItems in _context.BasketItems
                                     on courses.Id equals basketItems.CourseId
                                   join orders in _context.Orders
                                     on basketItems.OrderId equals orders.Id
                                   where orders.Status == true &&
                                         orders.UserId == userId
                                   select new { courses, basketItems, orders }
                                   ).Select(x => x.courses.Id).ToListAsync();
            return courseIds;
        }
    }
}