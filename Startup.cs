using FoodDonationWebApp.Data;
using FoodDonationWebApp.Helper;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Seed;
using FoodDonationWebApp.Services.Implementation;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace FoodDonationWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services) { 
            services.AddControllersWithViews();
            services.AddDbContext<FoodDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Data")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<FoodDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IFileServices, FileServices>();
            services.AddScoped<IDonation, DonationRepository>();
            services.AddScoped<IPickupRequestRepository, PickupRequestRepository>();



            services.ConfigureApplicationCookie(o =>
            {
                o.AccessDeniedPath = "/Auth/Login";
                o.LoginPath = "/Auth/Login";
                o.LogoutPath = "/Auth/Logout";
                o.Cookie.Name = "FoodWebApp";
                o.ExpireTimeSpan = TimeSpan.FromDays(30);
                o.SlidingExpiration = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseMiddleware<HandleExceptionMiddleware>();

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Login}/{id?}");
            });
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            SeedData.Initialize(serviceProvider, userManager).Wait();
        }

    }
}
