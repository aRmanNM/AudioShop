using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> CreateCoupon(Coupon coupon);
        Task<IEnumerable<Coupon>> GetCoupons(string[] codes);
        Task<Coupon> GetCouponByCode(string code);
        Task<bool> CheckCouponCodeExists(string CouponCode);
        Task<string> GenerateCouponCode();
    }
}
