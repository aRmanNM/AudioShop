using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StoreContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<OrderEpisode> OrderEpisodes { get; set; }
        public DbSet<BlacklistItem> Blacklist { get; set; }
        public DbSet<SliderItem> SliderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SalespersonCredential> SalespersonCredentials { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            builder.Entity<OrderEpisode>().HasKey(c => new {c.EpisodeId, c.OrderId});
            builder.Entity<BlacklistItem>().HasKey(c => new { c.CouponId, c.UserId });
        }
    }

}