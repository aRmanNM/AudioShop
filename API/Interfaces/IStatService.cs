using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IStatService
    {
        Task<List<Stat>> GetAllStatsInRange(DateTime start, DateTime end);
        Task<List<Stat>> GetAllStatsTotal();
    }
}