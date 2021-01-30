using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole> UserRoles { get; }

        public Role()
        {
            UserRoles = new Collection<UserRole>();
        }
    }
}