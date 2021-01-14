using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any() || !roleManager.Roles.Any())
            {
                var user = new User
                {
                    UserName = "Arman",
                };

                var roles = new List<Role>
                {
                    new Role{Name = "Member", NormalizedName = "MEMBER"},
                    new Role{Name = "Admin", NormalizedName = "ADMIN"},
                    new Role{Name = "SalesPerson", NormalizedName = "SALESPERSON"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }


                userManager.CreateAsync(user, "P@ssw0rd").Wait();
                userManager.AddToRoleAsync(user, "SalesPerson").Wait();


                var adminUser = new User
                {
                    UserName = "Admin",
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}