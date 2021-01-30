using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly StoreContext _context;

        public CheckoutRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Checkout>> GetCheckouts(bool status)
        {
            return await _context.Checkouts.Where(c => c.Status == status).ToArrayAsync();
        }

        public Checkout EditCheckout(Checkout checkout)
        {
            _context.Checkouts.Update(checkout);
            return checkout;
        }

        public async Task<Checkout> CreateCheckout(Checkout checkout)
        {
            await _context.Checkouts.AddAsync(checkout);
            return checkout;
        }

        public async Task<IEnumerable<Checkout>> GetCheckouts(string userId)
        {
            return await _context.Checkouts.Where(c => c.UserId == userId).ToArrayAsync();
        }
    }
}
