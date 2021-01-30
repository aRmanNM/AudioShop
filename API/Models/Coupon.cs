
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public int DiscountPercentage { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public ICollection<BlacklistItem> Blacklist { get; set; }

        public Coupon()
        {
            Blacklist = new Collection<BlacklistItem>();
        }
    }
}
