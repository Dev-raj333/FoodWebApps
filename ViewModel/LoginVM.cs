using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class LoginVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Please ennter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        public bool RememberMe { get; set; } = default!;
    }
}
