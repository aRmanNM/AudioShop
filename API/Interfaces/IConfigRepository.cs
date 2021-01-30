using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IConfigRepository
    {
         Task<Config> GetConfigAsync(string title);
         Task<IEnumerable<Config>> GetAllConfigsAsync();
         Task<Config> SetConfigAsync(Config config);
    }
}