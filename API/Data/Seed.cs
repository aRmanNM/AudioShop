using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Ads;
using API.Helpers;

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
                    Price = 300000,
                    Description = "توضیحاتی در خصوص چگونگی تقویت روابط زناشویی در ازدواج و اینکه کلا چطوریه داستان و باز یه مقدار توضیح دیگه صرفا برا طولانی شدن بیشتر ",
                    WaitingTimeBetweenEpisodes = 1,
                    IsActive = false,
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
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت سوم: بهبود روابط زناشی",
                            Description = "نکات نایاب در حل مشکلات زناشویی",
                            Sort = 2,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت چهارم: چگونگی فلان",
                            Description = "یک سوال مهم در خصوص ازدواج",
                            Sort = 3,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت پنجم: چگونه ازدواج کنید",
                            Description = "نکاتی حیاتی در خصوص ازدواج",
                            Sort = 4,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت ششم: بهبود روابط زناشی",
                            Description = "نکات نایاب در حل مشکلات زناشویی",
                            Sort = 5,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت هفتم: چگونگی فلان",
                            Description = "یک سوال مهم در خصوص ازدواج",
                            Sort = 6,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت هشتم: چگونه ازدواج کنید",
                            Description = "نکاتی حیاتی در خصوص ازدواج",
                            Sort = 7,
                            Price = 44000
                        },
                        new Episode {
                            Name = "قسمت نهم: بهبود روابط زناشی",
                            Description = "نکات نایاب در حل مشکلات زناشویی",
                            Sort = 8,
                            Price = 44000
                        }
                    }
                },
                new Course {
                    Name = "دوره موفقیت و کسب درآمد",
                    Price = 1200000,
                    Description = "چگونه به سادگی و در کمترین زمان بیشترین درآمد ممکن را داشته باشیم تنها با دانستن چند تکنیک ساده و آموزش آن بر عهده ما خواهد بود و اجرای آن توسط شما",
                    WaitingTimeBetweenEpisodes = 0,
                    IsActive = true,
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
                    WaitingTimeBetweenEpisodes = 2,
                    IsActive = true,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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
                },
                new Course {
                    Name = "پکیج کامل پرسونال برندینگ",
                    Price = 1000000,
                    Description = "پکج کامل شروع کار جهت گسترش برند شخصی و اینکه چطوری بتونیم تاثیرگذاری بیشتری داشته باشیم",
                    WaitingTimeBetweenEpisodes = 2,
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

        public static async Task SeedConfigs(StoreContext context)
        {
            if (context.Configs.Any())
            {
                return;
            }

            var configs = new List<Config> {
                // General
                new Config { TitleEn = "DefaultDiscountPercentage", Value = "20", TitleFa = "مقدار پیشفرض برای کد تخفیف", GroupEn = "General", GroupFa = "تنظیمات کلی", ValueType = ValueType.number },
                new Config { TitleEn = "DefaultSalespersonSharePercentage", Value = "10", TitleFa = "مقدار پیشفرض برای سهم فروشنده از هر خرید", GroupEn = "General", GroupFa = "تنظیمات کلی", ValueType = ValueType.number },
                new Config { TitleEn = "DefaultCheckoutThreshold", Value = "3000000", TitleFa = "حداقل میزان خرید برای ایجاد درخواست پرداخت", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.number },
                new Config { TitleEn = "LatestMobileAppName", Value = "audioshop1.0", TitleFa = "آخرین نسخه نرم افزار", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.text },
                new Config { TitleEn = "SubscriptionMonthlyFee", Value = "500000", TitleFa = "تعرفه اشتراک ماهیانه", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.number },
                new Config { TitleEn = "SubscriptionHalfYearlyFee", Value = "2500000", TitleFa = "تعرفه اشتراک شش ماهه", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.number },
                new Config { TitleEn = "SubscriptionYearlyFee", Value = "5000000", TitleFa = "تعرفه اشتراک سالیانه", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.number },
                new Config { TitleEn = "PhoneNumber", Value = "09121112222", TitleFa = "شماره تماس پشتیبانی", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.text },
                new Config { TitleEn = "IsAdsEnabled", Value = "1", TitleFa = "فعال بودن تبلیغات", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.boolean },
                new Config { TitleEn = "IsPopUpEnabled", Value = "1", TitleFa = "فعال بودن پاپ آپ تبلیغات تمام صفحه", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.boolean },
                new Config { TitleEn = "AboutUs", Value = "اینجا درباره ماست", TitleFa = "درباره ما", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.text },
                new Config { TitleEn = "SubscriptionPaidMessage", Value = "خرید اشتراک با موفقیت انجام شد", TitleFa = "پیام پیشفرض خرید اشتراک", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.text },
                new Config { TitleEn = "CoursePaidMessage", Value = "خرید محصول با موفقیت انجام شد", TitleFa = "پیام پیشفرض خرید محصول", GroupEn = "General", GroupFa = "تنظیمات کلی" , ValueType = ValueType.text },
                // Reminder
                new Config { TitleEn = "ReminderNotifTitle", Value = "یادآوری دوره", TitleFa = "عنوان اعلان", GroupEn = "Reminder", GroupFa = "اعلان یادآوری", ValueType = ValueType.text },
                new Config { TitleEn = "ReminderNotifBody", Value = "برو دوره تو گوش بده ادم حسابی", TitleFa = "بدنه اعلان", GroupEn = "Reminder", GroupFa = "اعلان یادآوری", ValueType = ValueType.text },
                new Config { TitleEn = "ReminderNotifTime", Value = "10", TitleFa = "ساعت اعلان", GroupEn = "Reminder", GroupFa = "اعلان یادآوری", ValueType = ValueType.number },
                new Config { TitleEn = "ReminderNotifCourseId", Value = "1", TitleFa = "کد دوره", GroupEn = "Reminder", GroupFa = "اعلان یادآوری", ValueType = ValueType.number },
                // Promote
                new Config { TitleEn = "PromoteNotifTitle", Value = "دوره جدید فلان", TitleFa = "عنوان اعلان", GroupEn = "Promote", GroupFa = "اعلان پروموت", ValueType = ValueType.text },
                new Config { TitleEn = "PromoteNotifBody", Value = "دوره جدید و جذاب فلان", TitleFa = "بدنه اعلان", GroupEn = "Promote", GroupFa = "اعلان پروموت", ValueType = ValueType.text},
                new Config { TitleEn = "PromoteNotifTime", Value = "11", TitleFa = "ساعت اعلان", GroupEn = "Promote", GroupFa = "اعلان پروموت", ValueType = ValueType.number },
                new Config { TitleEn = "PromoteNotifCourseId", Value = "2", TitleFa = "کد دوره", GroupEn = "Promote", GroupFa = "اعلان پروموت", ValueType = ValueType.number },


            };

            await context.Configs.AddRangeAsync(configs);
        }

        public static async Task SeedStats(StoreContext context)
        {
            if (context.Stats.Any())
            {
                return;
            }

            var stats = new List<Stat> {
                new Stat { TitleEn = "IPG", TitleFa = "ورود به درگاه پرداخت" },
                new Stat { TitleEn = "AppFirstPage", TitleFa = "ورود به صفحه راهنمای برنامه" },
            };

            await context.Stats.AddRangeAsync(stats);
        }

        public static async Task SeedPlaces(StoreContext context)
        {
            if (context.Places.Any())
            {
                return;
            }

            var places = new List<Place> {
                // Fullscreen
                new Place { TitleEn = "HomePage", TitleFa = "صفحه خانه", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "CoursePreview", TitleFa = "نمایش دوره یا کتاب", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "CoursePage", TitleFa = "صفحه دوره یا کتاب", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "Login-Cart", TitleFa = "صفحه لاگین سبد خرید", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "Login-Favorites", TitleFa = "صفحه لاگین علاقه مندی ها", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "Login-Profile", TitleFa = "صفحه لاگین پروفایل", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "NowPlaying", TitleFa = "صفحه پخش فایل", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "SignUp", TitleFa = "صفحه ثبت نام", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "AddSalesPersonCuponCode", TitleFa = "صفحه افزودن کوپن فروشنده", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "SupportPage", TitleFa = "صفحه فروشنده", AdType = AdType.Fullscreen, IsEnabled = true },
                new Place { TitleEn = "PsycologicalTests", TitleFa = "صفحه تست های روانشناسی", AdType = AdType.Fullscreen, IsEnabled = true },
                // Native
                new Place { TitleEn = "Loading-up", TitleFa = "صفحه لودینگ - بالا", AdType = AdType.Native, IsEnabled = true },
                new Place { TitleEn = "Loading-down", TitleFa = "صفحه لودینگ - پایین", AdType = AdType.Native, IsEnabled = true },
                new Place { TitleEn = "HomePage", TitleFa = "صفحه خانه", AdType = AdType.Native, IsEnabled = true },
                new Place { TitleEn = "Library", TitleFa = "صفحه کتابخانه", AdType = AdType.Native, IsEnabled = true },
                new Place { TitleEn = "Profile", TitleFa = "صفحه پروفایل", AdType = AdType.Native, IsEnabled = true },
                // Banner
                new Place { TitleEn = "HomePage-BelowSlider", TitleFa = "صفحه اصلی زیر اسلایدر", AdType = AdType.Banner, IsEnabled = true },
                new Place { TitleEn = "CoursePreview-Top", TitleFa = "صفحه نمایش دوره یا کتاب - بالا", AdType = AdType.Banner, IsEnabled = true },
                new Place { TitleEn = "CoursePreview-BelowAddToFavorite", TitleFa = "صفحه نمایش دوره یا کتاب - زیر علاقه مندی", AdType = AdType.Banner, IsEnabled = true },
                new Place { TitleEn = "HomePage-TopOfSilder", TitleFa = "صفحه اصلی بالا اسلایدر", AdType = AdType.Banner, IsEnabled = true },
            };

            await context.Places.AddRangeAsync(places);
        }
    }
}