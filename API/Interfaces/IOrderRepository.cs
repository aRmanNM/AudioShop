using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        // Task CreateBasketItems(IEnumerable<OrderCourse> basketItems);
        Task<Order> GetOrderById(int orderId);
        Task<PaginatedResult<OrderForSalespersonDto>> GetOrdersForCheckout(string couponCode,  int pageNumber = 1, int pageSize = 10);
    }
}