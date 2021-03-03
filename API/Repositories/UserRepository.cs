using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;

        public UserRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetSalespersonByCouponCode(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return null;
            }

            var userRole = await _context.UserRoles
                .Include(u => u.User).Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.User.CouponCode == couponCode && u.Role.Name == "Salesperson");

            return userRole.User;
        }

        public async Task<User> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return null;
            }

            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public Task<User> FindUserById(string userId)
        {
            return _context.Users
                .Include(u => u.SalespersonCredential).ThenInclude(sc => sc.IdCardPhoto)
                .Include(u => u.SalespersonCredential).ThenInclude(sc => sc.BankCardPhoto)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<PaginatedResult<User>> GetAllSalespersons(string search,
            bool onlyShowUsersWithUnacceptedCred = false, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > 20 || pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var salespersons = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Salesperson"));

            if (!string.IsNullOrEmpty(search))
            {
                salespersons = salespersons.Where(s => s.UserName.Contains(search) || s.LastName.Contains(search) || s.FirstName.Contains(search));
            }

            if (onlyShowUsersWithUnacceptedCred)
            {
                salespersons = salespersons.Where(s => s.CredentialAccepted == false);
            }

            var result = new PaginatedResult<User>();
            result.TotalItems = await salespersons.CountAsync();
            result.Items = await salespersons.OrderByDescending(c => c.Id)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToArrayAsync();

            return result;
        }
    }
}
