using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public FoodType FoodType { get; set; } 

        [Required]
        public int Quantity { get; set; }  

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(250)]
        public string Location { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
}
