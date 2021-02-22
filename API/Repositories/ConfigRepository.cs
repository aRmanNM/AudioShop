using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly StoreContext _context;
        public ConfigRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Config> GetConfigAsync(string titleEn)
        {
            return await _context.Configs.FirstOrDefaultAsync(c => c.TitleEn == titleEn);
        }

        public async Task<IEnumerable<Config>> GetAllConfigsAsync(string GroupEn)
        {
            return await _context.Configs.Where(c => c.GroupEn.ToUpper() == GroupEn.ToUpper()).ToListAsync();
        }

        public async Task<Config> SetConfigAsync(Config config)
        {
            await _context.Configs.AddAsync(config);
            return config;
        }


    }
}