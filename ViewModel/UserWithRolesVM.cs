using FoodDonationWebApp.Models;

namespace FoodDonationWebApp.ViewModel
{
    public class UserWithRolesVM
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}

