using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Models;
using API.Models.Options;
using API.Repositories;
using Microsoft.AspNetCore.StaticFiles;
using System;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddResponseCaching();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMapperService, MapperService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IProgressRepository, ProgressRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStatRepository, StatRepository>();
            services.AddScoped<IStatService, StatService>();
            services.AddScoped<ILandingRepository, LandingRepository>();
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                services.AddSingleton<ISMSService, FakeSMSService>();
            else
                services.AddSingleton<ISMSService, SMSService>();

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IZarinPalService, ZarinPalService>();
            services.AddSingleton<RandomService>();

            services.AddHttpClient();

            services.Configure<SMSOptions>(_config.GetSection("SMSOptions"));
            services.Configure<PhotoOptions>(_config.GetSection("PhotoOptions"));
            services.Configure<AudioOptions>(_config.GetSection("AudioOptions"));

            // services.AddDbContext<StoreContext>(x => x.UseInMemoryDatabase("StoreDb"));
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("Sqlite")));

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredUniqueChars = 1;
                opt.Password.RequiredLength = 6;
            }).AddRoles<Role>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<StoreContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
                    ValidIssuer = _config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod(); // FOR LOCAL CLIENT DEVELOPMENT ONLY!
                });
            });

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();
            app.UseRouting();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });


            app.UseCors();

            // app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
