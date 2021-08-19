using System;
using API.Helpers;
using API.Models;

namespace API.Dtos
{
    public class UserDto
    {
        public string Token { get; set; }
        public bool HasPhoneNumber { get; set; }
        public string SalespersonCouponCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public short Age { get; set; }
        public Gender Gender { get; set; }
        public bool Employed { get; set; }
        public DateTime SubscriptionExpirationDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
