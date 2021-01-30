using System.Threading.Tasks;
using API.Data;
using API.Interfaces;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            await _context.SaveChangesAsync();
            return 0;
        }
    }
}
