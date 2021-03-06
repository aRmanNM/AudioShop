﻿using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> CreateCoupon(Coupon coupon);
        Task<IEnumerable<Coupon>> GetCoupons(bool includeSalespersons = false);
        Task<Coupon> GetCouponByCode(string code);
        Task<bool> CheckUserIsBlacklisted(string couponCode, string userId);
        Task<string> GenerateCouponCode();
        void UpdateCoupon(Coupon coupon);
    }
}
