using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IStatRepository
    {
        Task SetStatByCode(StatName statName);
        Task<List<Stat>> GetAllStatsInRange(DateTime start, DateTime end);
        Task<List<Stat>> GetAllStatsTotal();
        Task<List<Stat>> GetSalesStatsInRange(DateTime start, DateTime end);
        Task<List<Stat>> GetSalesStatsTotal();
        Task<List<Stat>> GetRegisterationStatsInRange(DateTime start, DateTime end);
        Task<List<Stat>> GetRegisterationStatsTotal();

    }
}