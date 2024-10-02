using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.Models
{
    public class RecipientRequest
    {
        public int Id { get; set; }

        [ForeignKey("Recipient")]
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        [Required]
        [MaxLength(100)]
        public FoodType RequestedFoodType { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Required]
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
        public PickUpDrop pickupDrop { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }
    public enum PickUpDrop
    {
        PickUp,
        Drop
    }
}
