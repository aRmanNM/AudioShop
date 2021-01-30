using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        // Task CreateBasketItems(IEnumerable<OrderCourse> basketItems);
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetOrdersForCheckout(string couponCode);
    }
}