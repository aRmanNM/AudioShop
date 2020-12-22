using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}