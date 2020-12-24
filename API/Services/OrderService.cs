using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _context;
        public OrderService(StoreContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBasketItems(IEnumerable<BasketItem> basketItems)
        {
            await _context.AddRangeAsync(basketItems);
            await _context.SaveChangesAsync();
        }
    }
}