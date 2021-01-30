using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (userManager.Users.Any() || roleManager.Roles.Any())
            {
                return;
            }

            var adminUser = new User
            {
                UserName = "admin",
            };

            IdentityResult result = await userManager.CreateAsync(adminUser, "12345678910"); // TODO: set strong password!

            var roles = new List<Role>
                {
                    new Role{Name = "Member", NormalizedName = "MEMBER"},
                    new Role{Name = "Admin", NormalizedName = "ADMIN"},
                    new Role{Name = "Salesperson", NormalizedName = "SALESPERSON"}
                };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (result.Succeeded)
            {
                var admin = await userManager.FindByNameAsync("Admin");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        public static async Task SeedCourses(StoreContext context)
        {
            if (context.Courses.Any())
            {
                return;
            }

            var courses = new List<Course>() {
                new Course {
                    Name = "دوره آموزشی ازدواج و روابط زناشویی",
                    Price = 1000000,
                    Description = "توضیحاتی در خصوص چگونگی تقویت روابط زناشویی در ازدواج و اینکه کلا چطوریه داستان و باز یه مقدار توضیح دیگه صرفا برا طولانی شدن بیشتر ",
                    Episodes = new List<Episode>() {
                        new Episode {
                            Name = "قسمت اول: چگونگی فلان",
                            Description = "یک سوال مهم در خصوص ازدواج",
                            Sort = 0,
                            Price = 0
                        },
                        new Episode {
                            Name = "قسمت دوم: چگونه ازدواج کنید",
                            Description = "نکاتی حیاتی در خصوص ازدواج",
                            Sort = 1,
                            Price = 450000
                        },
                        new Episode {
                            Name = "قسمت سوم: بهبود روابط زناشی",
                            Description = "نکات نایاب در حل مشکلات زناشویی",
                            Sort = 2,
                            Price = 5800000
                        }
                    }
                },
                new Course {
                    Name = "دوره موفقیت و کسب درآمد",
                    Price = 1200000,
                    Description = "چگونه به سادگی و در کمترین زمان بیشترین درآمد ممکن را داشته باشیم تنها با دانستن چند تکنیک ساده و آموزش آن بر عهده ما خواهد بود و اجرای آن توسط شما",
                    Episodes = new List<Episode>() {
                        new Episode {
                            Name = "قسمت اول: چگونگی فلان",
                            Description = "چرا میخواهید پولدار شوید",
                            Sort = 0,
                            Price = 400000
                        },
                        new Episode {
                            Name = "تکنیک های کسب درآمد",
                            Description = "توضیح طولانی تر برای ارایه توضیحات بیشتر در خصوص این قسمت از دوره",
                            Sort = 1,
                            Price = 0
                        },
                        new Episode {
                            Name = "چگونه با بورس خیلی پولدار شویم",
                            Description = "نکات خیلی خیلی ظریف در خصوص خرید و فروش در بورس",
                            Sort = 2,
                            Price = 900000
                        }
                    }
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    Episodes = new List<Episode>() {
                        new Episode {
                            Name = "قسمت اول: چگونگی فلان",
                            Description = "|رسونال برندینگ چیه",
                            Sort = 0,
                            Price = 400000
                        },
                        new Episode {
                            Name = "چطوری شروع کنیم",
                            Description = "از کدوم شبکه اجتماعی شروع کنیم",
                            Sort = 1,
                            Price = 800000
                        },
                        new Episode {
                            Name = "توضیحات نهایی",
                            Description = "جمع بندی نکات مهم آموزش",
                            Sort = 2,
                            Price = 0
                        },
                        new Episode {
                            Name = "ادامه مسیر",
                            Description = "معروف شدیم بعدش چی!",
                            Sort = 3,
                            Price = 0
                        }
                    }
                }
            };

            await context.Courses.AddRangeAsync(courses);
        }

        public static async Task SeedConfigs(StoreContext context) {
            if (context.Configs.Any())
            {
                return;
            }

            var configs = new List<Config> {
                new Config { Title = "DefaultDiscountPercentage", Value = "20", Description = "مقدار پیشفرض برای کد تخفیف" },
                new Config { Title = "DefaultSalespersonSharePercentage", Value = "10", Description = "مقدار پیشفرض برای سهم فروشنده از هر خرید" }
            };

            await context.Configs.AddRangeAsync(configs);
        }
    }
}