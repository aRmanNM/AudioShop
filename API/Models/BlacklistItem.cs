using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class BlacklistItem
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public string CouponCode { get; set; }
    }
}
