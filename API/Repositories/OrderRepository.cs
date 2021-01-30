using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;
        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await _context.AddAsync(order);
            return order;
        }

        // public async Task CreateBasketItems(IEnumerable<OrderCourse> basketItems)
        // {
        //     await _context.AddRangeAsync(basketItems);
        // }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Coupons)
                .Include(o => o.Episodes)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersForCheckout(int couponId)
        {
            return await _context.Orders
                .Include(o => o.Episodes)
                .Include(o => o.Coupons)
                .Where(o => o.Coupons.Any(c => c.Id == couponId) && o.Status == true)
                .ToArrayAsync();
        }
    }
}