using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IConfigRepository
    {
         Task<Config> GetConfigAsync(string titleEn);
         Task<IEnumerable<Config>> GetConfigsByGroupAsync(string GroupEn);
         Config SetConfig(Config config);
         Task<IEnumerable<Config>> GetAllConfigsAsync();
    }
}