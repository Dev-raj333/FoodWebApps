using FoodDonationWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDonationWebApp.Data
{
    public class FoodDbContext : IdentityDbContext<ApplicationUser>
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }    
        public DbSet<Donation> Donations { get; set; }
        public DbSet<PickupRequest> PickupRequests { get; set; }
        public DbSet<DropRequest> DropRequests { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<RecipientRequest> RecipientRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
        }
    }
}
