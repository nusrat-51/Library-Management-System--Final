using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class FinePayment
    {
        public int Id { get; set; }

        // Link payment to a borrow/application record (recommended)
        public int BookApplicationId { get; set; }

        // ✅ ADD THIS (navigation + FK mapping)
        [ForeignKey(nameof(BookApplicationId))]
        public BookApplication? BookApplication { get; set; }

        // Who is paying
        public long StudentId { get; set; }

        [MaxLength(256)]
        public string StudentEmail { get; set; } = "";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string TranId { get; set; } = "";

        [MaxLength(50)]
        public string Status { get; set; } = "Initiated";

        [MaxLength(50)]
        public string Gateway { get; set; } = "SSLCommerz";

        [MaxLength(100)]
        public string? ValId { get; set; }

        [MaxLength(100)]
        public string? BankTranId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? PaidAt { get; set; }
    }
}
