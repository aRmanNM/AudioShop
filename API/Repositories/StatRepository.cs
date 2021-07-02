using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class StatRepository : IStatRepository
    {
        private readonly StoreContext _context;
        public StatRepository(StoreContext context)
        {
            _context = context;

        }

        public async Task<List<Stat>> GetAllStatsInRange(DateTime start, DateTime end)
        {
            var mem = await _context.Stats
                .AsNoTracking()
                .Where(s => s.DateOfStat.Date >= start.Date && s.DateOfStat.Date <= end.Date)
                .ToListAsync();

            var stats = new List<Stat>();

            for (int i = 0; i <= (end.DayOfYear - start.DayOfYear); i++)
            {
                var ipgStat = mem.FirstOrDefault(s => s.DateOfStat.Date == start.Date.AddDays(i) && s.TitleEn == "IPG");
                if (ipgStat != null) stats.Add(ipgStat);
                else stats.Add(new Stat {
                    TitleEn = "IPG",
                    TitleFa = "آمار روزانه ورود به درگاه پرداخت",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = 0
                });

                var firstPageStat = mem.FirstOrDefault(s => s.DateOfStat.Date == start.Date.AddDays(i) && s.TitleEn == "AppFirstPage");
                if (firstPageStat != null) stats.Add(firstPageStat);
                else stats.Add(new Stat {
                    TitleEn = "AppFirstPage",
                    TitleFa = "آمار روزانه ورود به صفحه راهنمای برنامه",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = 0
                });
            }

            return stats;
        }

        public async Task<List<Stat>> GetAllStatsTotal()
        {
            var statsTemplates = await _context.Stats
                .Select(s => new { s.TitleEn, s.TitleFa })
                .Distinct()
                .ToArrayAsync();

            var result = new List<Stat>();

            foreach (var item in statsTemplates)
            {
                var stat = new Stat
                {
                    TitleEn = item.TitleEn,
                    TitleFa = item.TitleFa,
                    Counter = _context.Stats.Where(s => s.TitleEn == item.TitleEn).Sum(s => s.Counter)
                };

                result.Add(stat);
            }

            return result;
        }

        public async Task<List<Stat>> GetRegisterationStatsInRange(DateTime start, DateTime end)
        {
            var users = await _context.Users.AsNoTracking().Where(u => u.CreatedAt.Date >= start && u.CreatedAt.Date <= end).ToListAsync();

            var stats = new List<Stat>();

            for (int i = 0; i <= (end.DayOfYear - start.DayOfYear); i++)
            {
                stats.Add(new Stat
                {
                    TitleEn = "RegisterCount",
                    TitleFa = "تعداد ثبت نام کاربران",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = users.Where(o => o.CreatedAt.Date == start.AddDays(i)).Count()
                });

                stats.Add(new Stat
                {
                    TitleEn = "PhoneRegisterCount",
                    TitleFa = "تعداد شماره های تایید شده",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = users.Where(o => o.CreatedAt.Date == start.AddDays(i) && o.PhoneNumberConfirmed).Count()
                });
            }

            return stats;
        }

        public async Task<List<Stat>> GetRegisterationStatsTotal()
        {
            var stats = new List<Stat>();

            stats.AddRange(new List<Stat> {
                new Stat {
                    TitleEn = "TotalRegisterCount",
                    TitleFa = "تعداد ثبت نام کاربران",
                    Counter = await _context.Users.CountAsync()
                },
                new Stat {
                    TitleEn = "TotalPhoneRegisterCount",
                    TitleFa = "تعداد شماره های تایید شده",
                    Counter = await _context.Users.Where(o => o.PhoneNumberConfirmed).CountAsync()
                }
            });

            return stats;
        }

        public async Task<List<Stat>> GetSalesStatsInRange(DateTime start, DateTime end)
        {
            var orders = await _context.Orders.AsNoTracking().Where(o => o.Date.Date >= start && o.Date.Date <= end).ToListAsync();

            var stats = new List<Stat>();

            for (int i = 0; i <= (end.DayOfYear - start.DayOfYear); i++)
            {
                stats.Add(new Stat
                {
                    TitleEn = "SuccessfulOrdersCount",
                    TitleFa = "تعداد پرداخت های موفق",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = orders.Where(o => o.Date.Date == start.AddDays(i) && o.Status).Count()
                });

                stats.Add(new Stat
                {
                    TitleEn = "SuccessfulOrdersSum",
                    TitleFa = "مجموع پرداخت های موفق",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = (int)orders.Where(o => o.Date.Date == start.AddDays(i) && o.Status).Sum(o => o.PriceToPay)
                });

                stats.Add(new Stat
                {
                    TitleEn = "FailedOrdersCount",
                    TitleFa = "تعداد پرداخت های ناموفق",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = orders.Where(o => o.Date.Date == start.AddDays(i) && !o.Status).Count()
                });

                stats.Add(new Stat
                {
                    TitleEn = "FailedOrdersSum",
                    TitleFa = "مجموع پرداخت های ناموفق",
                    DateOfStat = start.Date.AddDays(i),
                    Counter = (int)orders.Where(o => o.Date.Date == start.AddDays(i) && !o.Status).Sum(o => o.PriceToPay)
                });
            }

            return stats;
        }

        public async Task<List<Stat>> GetSalesStatsTotal()
        {
            var stats = new List<Stat>();

            stats.AddRange(new List<Stat> {
                new Stat {
                    TitleEn = "TotalFailedOrdersCount",
                    TitleFa = "تعداد کل پرداخت های ناموفق",
                    Counter = await _context.Orders.Where(o => !o.Status).CountAsync()
                },
                new Stat {
                    TitleEn = "TotalFailedOrdersSum",
                    TitleFa = "مجموع کل پرداخت های ناموفق",
                    Counter = (int)(await _context.Orders.Where(o => !o.Status).SumAsync(o => o.PriceToPay))
                },
                new Stat {
                    TitleEn = "TotalSuccessfulOrdersCount",
                    TitleFa = "تعداد کل پرداخت های موفق",
                    Counter = await _context.Orders.Where(o => o.Status).CountAsync()
                },
                new Stat {
                    TitleEn = "TotalSuccessfulOrdersSum",
                    TitleFa = "مجموع کل پرداخت های موفق",
                    Counter = (int)(await _context.Orders.Where(o => o.Status).SumAsync(o => o.PriceToPay))
                }
            });

            return stats;
        }

        public async Task SetStatByCode(StatName statName)
        {
            var stat = await _context.Stats
                .FirstOrDefaultAsync(s => s.TitleEn == statName.ToString() && s.DateOfStat.DayOfYear == DateTime.Today.DayOfYear);

            if (stat == null)
            {
                var refStat = await _context.Stats.FirstOrDefaultAsync(s => s.TitleEn == statName.ToString());

                await _context.Stats.AddAsync(new Stat
                {
                    TitleEn = statName.ToString(),
                    TitleFa = refStat.TitleFa,
                    Counter = 1,
                    DateOfStat = DateTime.Today.Date
                });

                return;
            }

            stat.Counter++;
        }
    }
}