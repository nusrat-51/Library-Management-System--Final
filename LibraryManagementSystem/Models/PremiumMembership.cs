using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class PremiumMembership
    {
        public int Id { get; set; }

        // Your Identity user id is long
        public long StudentId { get; set; }

        [Required]
        public string StudentEmail { get; set; } = "";

        // Store hashed barcode, not raw (security)
        [Required]
        public string BarcodeHash { get; set; } = "";

        public bool IsPurchased { get; set; } = false;

        public DateTime? PurchasedAt { get; set; }

        // Optional (you can ignore now)
        public DateTime? ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
