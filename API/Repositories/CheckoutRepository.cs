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

        public async Task<IEnumerable<Checkout>> GetCheckoutsAsync(bool status, string userName, bool includeSalespersonInfo = false)
        {
            var checkouts = _context.Checkouts.AsQueryable();

            if (includeSalespersonInfo)
            {
                checkouts = checkouts.Include(c => c.User).ThenInclude(u => u.SalespersonCredential);
            }

            if(!string.IsNullOrEmpty(userName))
            {
                checkouts = checkouts.Where(c => c.UserName == userName);
            }

            return await checkouts.Where(c => c.Status == status).ToArrayAsync();
        }

        public Checkout EditCheckout(Checkout checkout)
        {
            _context.Checkouts.Update(checkout);
            return checkout;
        }

        public async Task<Checkout> CreateCheckoutAsync(Checkout checkout)
        {
            await _context.Checkouts.AddAsync(checkout);
            return checkout;
        }

        public async Task<IEnumerable<Checkout>> GetSalespersonCheckoutsAsync(string userId)
        {
            return await _context.Checkouts
                .Include(c => c.ReceiptPhoto)
                .Where(c => c.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<Checkout> GetCheckoutWithIdAsync(int checkoutId)
        {
            return await _context.Checkouts
                .Include(c => c.User)
                .ThenInclude(u => u.SalespersonCredential)
                .ThenInclude(sc => sc.IdCardPhoto)
                .Include(c => c.User)
                .ThenInclude(u => u.SalespersonCredential)
                .ThenInclude(sc => sc.BankCardPhoto)
                .Include(c => c.ReceiptPhoto)
                .FirstOrDefaultAsync(c => c.Id == checkoutId);
        }
    }
}
