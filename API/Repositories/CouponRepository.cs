﻿using System.Collections.Generic;
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

        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            return coupon;
        }

        public async Task<Coupon> GetCouponByCodeAsync(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return null;
            }

            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode);
        }

        public async Task<bool> CheckUserIsBlacklistedAsync(string couponCode, string userId)
        {
            return await _context.Blacklist.FirstOrDefaultAsync(i =>
                i.CouponCode == couponCode && i.UserId == userId) != null;
        }

        public async Task<IEnumerable<Coupon>> GetCouponsAsync(bool includeSalespersons = false)
        {
            if (includeSalespersons)
            {
                return await _context.Coupons.ToArrayAsync();
            }

            return await _context.Coupons.Where(c => string.IsNullOrEmpty(c.UserId)).ToArrayAsync();
        }

        public async Task<string> GenerateCouponCodeAsync()
        {
            string code;
            do
            {
                code = RandomString.Generate();
            } while (await CheckCouponExists(code) == true);

            return code;
        }

        private async Task<bool> CheckCouponExists(string couponCode)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode) != null;
        }

        public void UpdateCoupon(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
        }
    }
}
