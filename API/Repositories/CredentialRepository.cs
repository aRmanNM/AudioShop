using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly StoreContext _context;
        public CredentialRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<SalespersonCredential> CreateCredentialAsync(SalespersonCredential salespersonCredential)
        {
            await _context.SalespersonCredentials.AddAsync(salespersonCredential);
            return salespersonCredential;
        }

        public async Task<SalespersonCredential> GetSalespersonCredentialAsync(string userId, bool withTracking = false)
        {
            var credential = _context.SalespersonCredentials
                .Include(sc => sc.IdCardPhoto)
                .Include(sc => sc.BankCardPhoto);

            if (withTracking)
            {
                return await credential.FirstOrDefaultAsync(sc => sc.UserId == userId);
            }
            else
            {
                return await credential.AsNoTracking().FirstOrDefaultAsync(sc => sc.UserId == userId);
            }
        }

        public SalespersonCredential UpdateCredential(SalespersonCredential salespersonCredential)
        {
            _context.SalespersonCredentials.Update(salespersonCredential);
            return salespersonCredential;
        }
    }
}