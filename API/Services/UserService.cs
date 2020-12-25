using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly StoreContext _context;
        public UserService(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetUserCourses(string userId)
        {
            var orderIds = await _context.Orders.Where(x => x.Status == true).Select(x => x.Id).ToListAsync();
            var basketitems = await _context.BasketItems.Where(x => orderIds.Contains(x.OrderId)).Select(x => x.CourseId).ToListAsync();
            var courses = await _context.Courses.Where(t => basketitems.Contains(t.Id)).ToListAsync();
            return courses;
        }
    }
}