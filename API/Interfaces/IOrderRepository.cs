using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId, bool withUserInfo = false);
        Task<PaginatedResult<OrderForSalespersonDto>> GetOrdersForCheckoutAsync(string couponCode, int pageNumber = 1, int pageSize = 10);
    }
}