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

        public async Task<IEnumerable<Config>> GetAllConfigs()
        {
            return await _context.Configs.ToArrayAsync();
        }

        public async Task<Config> GetConfig(string titleEn)
        {
            return await _context.Configs.FirstOrDefaultAsync(c => c.TitleEn == titleEn);
        }

        public async Task<IEnumerable<Config>> GetConfigsByGroup(string GroupEn)
        {
            return await _context.Configs.Where(c => c.GroupEn.ToUpper() == GroupEn.ToUpper()).ToListAsync();
        }

        public Config SetConfig(Config config)
        {
            _context.Configs.Update(config);
            return config;
        }


    }
}