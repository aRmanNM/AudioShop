using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
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

        public async Task<User> GetSalespersonByCouponCodeAsync(string couponCode)
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

        public Task<User> FindUserByIdAsync(string userId)
        {
            return _context.Users
                .Include(u => u.SalespersonCredential).ThenInclude(sc => sc.IdCardPhoto)
                .Include(u => u.SalespersonCredential).ThenInclude(sc => sc.BankCardPhoto)
                .Include(u => u.Coupon)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<PaginatedResult<User>> GetSalespersonsAsync(string search,
            SalespersonCredStatus? status, int pageNumber = 1, int pageSize = 10)
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
                .Include(u => u.Coupon)
                .Include(u => u.SalespersonCredential)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Salesperson"));

            if (!string.IsNullOrEmpty(search))
            {
                salespersons = salespersons.Where(s => s.UserName.Contains(search) || s.LastName.Contains(search) || s.FirstName.Contains(search));
            }

            if (status == SalespersonCredStatus.accepted)
            {
                salespersons = salespersons.Where(s => s.CredentialAccepted == true);
            } else if (status == SalespersonCredStatus.pending)
            {
                salespersons = salespersons.Where(s => s.CredentialAccepted == false && s.SalespersonCredential != null);
            } else if (status == SalespersonCredStatus.unaccepted)
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

        public async Task<User> FindUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}
