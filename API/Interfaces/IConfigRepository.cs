using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IConfigRepository
    {
         Task<Config> GetConfig(string titleEn);
         Task<IEnumerable<Config>> GetConfigsByGroup(string GroupEn);
         Config SetConfig(Config config);
         Task<IEnumerable<Config>> GetAllConfigs();
    }
}