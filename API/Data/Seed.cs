using System.Collections.Generic;
using System.Linq;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace API.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User{
                  DisplayName = "Arman",
                  UserName = "Arman",
                  Email = "arman@test.com"
                };

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "SalesPerson"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }


                userManager.CreateAsync(user, "P@ssw0rd").Wait();
                userManager.AddToRoleAsync(user, "Member").Wait();


                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "admin@test.com"
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