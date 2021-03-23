using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        Task<IEnumerable<Coupon>> GetCouponsAsync(bool includeSalespersons = false);
        Task<Coupon> GetCouponByCodeAsync(string code);
        Task<bool> CheckUserIsBlacklistedAsync(string couponCode, string userId);
        Task<string> GenerateCouponCodeAsync();
        void UpdateCoupon(Coupon coupon);
    }
}
