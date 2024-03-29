using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using API.Helpers;
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
        public EmploymentStatus Employed { get; set; }

        public int SalePercentageOfSalesperson { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal CurrentSalesOfSalesperson { get; set; }
        [Column(TypeName = "decimal(18)")]
        public decimal TotalSalesOfSalesperson { get; set; }

        public string VerificationToken { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Today;

        public string CouponCode { get; set; }
        public Coupon Coupon { get; set; }
        public SalespersonCredential SalespersonCredential { get; set; }
        public bool CredentialAccepted { get; set; } = false;

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Order> Orders { get; }
        public ICollection<BlacklistItem> Blacklist { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public ICollection<Favorite> FavoriteCoursesAndEpisodes { get; set; }
        public ICollection<Progress> ListeningsProgress { get; set; }

        public DateTime SubscriptionExpirationDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public UserType UserType { get; set; }

        public User()
        {
            UserRoles = new Collection<UserRole>();
            Orders = new Collection<Order>();
            Blacklist = new Collection<BlacklistItem>();
            Reviews = new Collection<Review>();
            FavoriteCoursesAndEpisodes = new Collection<Favorite>();
            ListeningsProgress = new Collection<Progress>();
        }
    }

    public enum Gender
    {
        Undefined, Male, Female
    }
}