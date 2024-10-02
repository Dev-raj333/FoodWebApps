using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class EditUserVM
    {
        public string Id { get; set; }  // Hidden field for the User's ID

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string SelectedRole { get; set; }

        public List<SelectListItem>? Roles { get; set; }

        public string? ProilePicture { get; set; }
    }
}
