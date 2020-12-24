using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Services
{
    public interface IOrderService
    {
        Task CreateOrder(Order order);
        Task CreateBasketItems(IEnumerable<BasketItem> basketItems);
    }
}