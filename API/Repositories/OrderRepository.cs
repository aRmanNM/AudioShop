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

        public async Task<PaginatedResult<OrderForSalespersonDto>> GetOrdersForCheckout(string couponCode, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > 20 || pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var orders = _context.Orders
                .Include(o => o.OrderEpisodes)
                .ThenInclude(oe => oe.Episode)
                .ThenInclude(oee => oee.Course)
                .Where(o => o.Status == true && o.SalespersonCouponCode == couponCode).AsQueryable();

            var totlaItems = orders.Count();
            var result = await orders.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(o => new { o.PriceToPay, o.Date, o.SalespersonShare, o.OrderEpisodes })
                .AsNoTracking()                
                .ToArrayAsync();

            var OrderForSalespersonDtos = result.Select(o => new OrderForSalespersonDto
            {
                Price = o.PriceToPay,
                Date = o.Date,
                SalespersonShareAmount = o.SalespersonShare,
                Courses = o.OrderEpisodes.Select(oe => oe.Episode.Course.Name).Distinct().ToArray()
            });

            return new PaginatedResult<OrderForSalespersonDto>
            {
                TotalItems = totlaItems,
                Items = OrderForSalespersonDtos
            };
        }
    }
}