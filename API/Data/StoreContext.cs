using API.Models;
using API.Models.Ads;
using API.Models.Landing;
using API.Models.Messages;
using API.Models.Tickets;
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
        public DbSet<ContentFile> ContentFiles { get; set; }
        public DbSet<OrderEpisode> OrderEpisodes { get; set; }
        public DbSet<BlacklistItem> Blacklist { get; set; }
        public DbSet<SliderItem> SliderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SalespersonCredential> SalespersonCredentials { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Stat> Stats { get; set; }
        public DbSet<Landing> Landings { get; set; }
        public DbSet<LandingPhoneNumber> LandingPhoneNumbers { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdPlace> AdPlaces { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

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
            builder.Entity<CourseCategory>().HasKey(c => new { c.CourseId, c.CategoryId });
            builder.Entity<AdPlace>().HasKey(c => new { c.AdId, c.PlaceId });
            builder.Entity<UserMessage>().HasKey(c => new { c.UserId, c.MessageId });
        }
    }

}