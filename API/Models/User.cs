using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public short Age { get; set; }
        public Gender Gender { get; set; }

        public int SalePercentage { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal CurrentSales { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal TotalSales { get; set; }

        public string VerificationToken { get; set; }

        public int? CouponId { get; set; }
        public Coupon Coupon { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Order> Orders { get; }

        public User()
        {
            UserRoles = new Collection<UserRole>();
            Orders = new Collection<Order>();
        }
    }

    public enum Gender
    {
        Male, Female
    }
}