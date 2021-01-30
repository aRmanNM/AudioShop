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
    public class CouponRepository : ICouponRepository
    {
        private readonly StoreContext _context;

        public CouponRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Coupon> CreateCoupon(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            return coupon;
        }

        public async Task<Coupon> GetCouponByCode(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return null;
            }

            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode);
        }

        public async Task<bool> CheckCouponCodeExists(string couponCode)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode) != null;
        }

        public async Task<string> GenerateCouponCode()
        {
            string code;
            do
            {
                code = RandomString.Generate();
            } while (await CheckCouponCodeExists(code) == true);

            return code;
        }

        public async Task<IEnumerable<Coupon>> GetCoupons(string[] codes)
        {
            return await _context.Coupons.Where(c => codes.Any(co => co == c.Code)).ToArrayAsync();
        }
    }
}
