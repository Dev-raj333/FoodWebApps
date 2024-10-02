﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDonationWebApp.Models
{
    public class PickupRequest
    {
        public int Id { get; set; }

        [ForeignKey("Donation")]
        public int DonationID { get; set; }
        public Donation Donation { get; set; }

        [ForeignKey("Volunteer")]
        public string VolunteerID { get; set; }
        public ApplicationUser Volunteer { get; set; }
        public DateTime PickupTime { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }

        public PickupStatus PickupStatus { get; set; } = PickupStatus.Scheduled;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
    }

    public enum PickupStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
}
