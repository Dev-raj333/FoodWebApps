using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class RegisterVM
    {
        public string? Id { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password not matched")]
        public string ConfirmPassword { get; set; } = default!;

        [Required(ErrorMessage = "Role is required")]
        public string SelectedRole { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNo { get; set; }

        [Display(Name = "Profile")]
        public IFormFile? Profile { get; set; }

    }
}
