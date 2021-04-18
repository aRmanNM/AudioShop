using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetSalespersonByCouponCodeAsync(string couponCode);
        Task<User> FindUserByPhoneNumberAsync(string phoneNumber);
        Task<User> FindUserByIdAsync(string userId);
        Task<PaginatedResult<User>> GetSalespersonsAsync(string search, SalespersonCredStatus? status, int pageNumber = 1, int pageSize = 10);
    }
}
