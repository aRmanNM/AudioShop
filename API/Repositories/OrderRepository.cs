using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
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
            await _context.Orders.AddAsync(order);
            return order;
        }

        // public async Task CreateBasketItems(IEnumerable<OrderCourse> basketItems)
        // {
        //     await _context.AddRangeAsync(basketItems);
        // }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderEpisodes)
                .ThenInclude(oe => oe.Episode)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<OrderForSalespersonDto>> GetOrdersForCheckout(string couponCode)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderEpisodes)
                .ThenInclude(oe => oe.Episode)
                .ThenInclude(oee => oee.Course)
                .Where(o => o.Status == true && o.SalespersonCouponCode == couponCode)
                .Select(o => new {o.PriceToPay, o.Date, o.SalespersonShare, o.OrderEpisodes})
                .ToArrayAsync();

            var OrderForSalespersonDtos = orders.Select(o => new OrderForSalespersonDto {
                Price = o.PriceToPay,
                Date = o.Date,
                SalespersonShareAmount = o.SalespersonShare,
                Courses = o.OrderEpisodes.Select(oe => oe.Episode.Course.Name).ToArray()
            });

            return OrderForSalespersonDtos;
        }
    }
}