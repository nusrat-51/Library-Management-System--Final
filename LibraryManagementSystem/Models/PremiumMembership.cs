using System;

namespace LibraryManagementSystem.Models
{
    public class PremiumMembership
    {
        public int Id { get; set; }

        public long StudentId { get; set; }              // optional but useful if you have it
        public string StudentName { get; set; } = "";
        public string StudentEmail { get; set; } = "";

        public string BarcodeHash { get; set; } = "";

        public bool IsPurchased { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? PurchasedAt { get; set; }
    }
}
