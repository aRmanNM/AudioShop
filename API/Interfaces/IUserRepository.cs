using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetSalespersonByCouponCode(string couponCode);
        Task<User> FindUserByPhoneNumberAsync(string phoneNumber);
        Task<User> FindUserById(string userId);
        Task<PaginatedResult<User>> GetAllSalespersons(string search, bool onlyShowUsersWithUnacceptedCred = false , int pageNumber = 1, int pageSize = 10);
    }
}
