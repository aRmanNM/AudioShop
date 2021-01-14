using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public string InvitationCode { get; set; }
        public string VerificationToken { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}