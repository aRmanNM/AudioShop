using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;
        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(Order order)
        {
            await _context.AddAsync(order);
            await SaveChanges();
        }

        public async Task CreateBasketItems(IEnumerable<BasketItem> basketItems)
        {
            await _context.AddRangeAsync(basketItems);
            await SaveChanges();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task SaveChanges(){
            await _context.SaveChangesAsync();
        }
    }
}