using Microsoft.AspNetCore.Identity;

namespace FoodDonationWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? PhoenNumber { get; set; }
        public string? Address { get; set; }
        
        public string? Profile { get; set; }
    }
}
