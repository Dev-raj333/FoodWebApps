using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDonationWebApp.Models
{
    public class Donation
    {
        public int Id { get; set; }
        [ForeignKey("Doner")]
        public string DonorID { get; set; }
        public ApplicationUser? Donor {  get; set; }
        public FoodType FoodType { get; set; }
        
        public int Quantity { get; set; }
        public DateTime? ExpiraryDate { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DonationStatus Status { get; set; } = DonationStatus.Available;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt {  get; set; } = DateTime.Now;

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
