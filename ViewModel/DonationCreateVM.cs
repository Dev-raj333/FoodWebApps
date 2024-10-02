using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class DonationCreateVM
    {
        [Required]
        public string DonorID { get; set; }

        [Required]
        [Display(Name = "Food Type")]
        public FoodType FoodType { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpiraryDate { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description of Donation")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Donation Status")]
        public DonationStatus Status { get; set; } = DonationStatus.Available;
    }

    public enum DonationStatus
    {
        Available,
        Pending,
        PickedUp,
        Completed
    }

    public enum FoodType
    {
        Fresh,
        Packaged,
        Bakery,
        Other
    }

}
