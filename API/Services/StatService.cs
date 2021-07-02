using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class StatService : IStatService
    {
        private readonly IStatRepository _statRepository;
        public StatService(IStatRepository statRepository)
        {
            _statRepository = statRepository;

        }

        public async Task<List<Stat>> GetAllStatsInRange(DateTime start, DateTime end)
        {
            var stats = new List<Stat>();
            stats.AddRange(await _statRepository.GetAllStatsInRange(start, end));
            stats.AddRange(await _statRepository.GetSalesStatsInRange(start, end));
            stats.AddRange(await _statRepository.GetRegisterationStatsInRange(start, end));

            return stats;
        }

        public async Task<List<Stat>> GetAllStatsTotal()
        {
            var stats = new List<Stat>();
            stats.AddRange(await _statRepository.GetAllStatsTotal());
            stats.AddRange(await _statRepository.GetSalesStatsTotal());
            stats.AddRange(await _statRepository.GetRegisterationStatsTotal());

            return stats;
        }
    }
}