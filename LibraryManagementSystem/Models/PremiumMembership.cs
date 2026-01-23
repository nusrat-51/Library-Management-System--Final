using System;

namespace LibraryManagementSystem.Models
{
    public class PremiumMembership
    {
        public int Id { get; set; }

        // -------------------------
        // Student Info
        // -------------------------
        public long StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public string StudentEmail { get; set; } = "";

        // -------------------------
        // Barcode unlock
        // -------------------------
        public string BarcodeHash { get; set; } = "";

        // -------------------------
        // Membership status
        // -------------------------
        public bool IsPurchased { get; set; }

        // IMPORTANT:
        // Keep CreatedAt non-nullable, but DO NOT assume DB always fills it.
        // If your DB column is nullable and contains NULLs, you must fix DB or make this nullable too.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // DB contains NULLs here (your screenshot), so this must be nullable
        public DateTime? PurchasedAt { get; set; }

        // =====================================================
        // Payment fields (SSLCommerz / Cash)
        // =====================================================
        public string Status { get; set; } = "Initiated";   // Initiated / Paid / Failed / Cancelled
        public decimal Amount { get; set; } = 0m;
        public string TranId { get; set; } = "";
        public string Gateway { get; set; } = "SSLCommerz";
        public string ValId { get; set; } = "";

        // =====================================================
        // Membership validity fields
        // =====================================================
        public int DurationDays { get; set; } = 30;

        // These are nullable because DB can store NULL until payment completes
        public DateTime? PaidAt { get; set; }              // when paid
        public DateTime? StartDate { get; set; }           // membership start
        public DateTime? EndDate { get; set; }             // membership end/expiry

        // Your table shows an "Expiry" column (NULL). Keep it nullable to prevent SqlNullValueException.
        // If you don't use it in code, it's still safe to keep here.
        public DateTime? Expiry { get; set; }

        // Optional (if you use it somewhere)
        public DateTime? ExpireAt { get; set; }
    }
}
