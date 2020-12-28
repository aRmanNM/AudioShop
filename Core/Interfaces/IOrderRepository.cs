using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task CreateBasketItems(IEnumerable<BasketItem> basketItems);
        Task<Order> GetOrderById(int orderId);
        Task SaveChanges();
    }
}