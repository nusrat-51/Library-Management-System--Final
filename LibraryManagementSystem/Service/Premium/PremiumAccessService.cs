using System.Security.Cryptography;
using System.Text;
using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service
{
    public interface IPremiumAccessService
    {
        Task<bool> HasPurchasedMembershipAsync(long studentId, CancellationToken ct);
        Task<bool> ValidateBarcodeAsync(long studentId, string barcodeRaw, CancellationToken ct);
        string HashBarcode(string barcodeRaw);
    }

    public class PremiumAccessService : IPremiumAccessService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public PremiumAccessService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // ✅ Check if student has purchased premium membership
        public async Task<bool> HasPurchasedMembershipAsync(long studentId, CancellationToken ct)
        {
            return await _db.PremiumMemberships
                .AsNoTracking()
                .AnyAsync(x => x.StudentId == studentId && x.IsPurchased, ct);
        }

        // ✅ Validate barcode entered by student
        public async Task<bool> ValidateBarcodeAsync(long studentId, string barcodeRaw, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(barcodeRaw))
                return false;

            barcodeRaw = barcodeRaw.Trim();

            // ✅ DEMO / BACKUP: allow unlock code from appsettings.json
            // appsettings.json:
            // "Premium": { "UnlockBarcode": "1234", "BarcodeSalt": "yoursalt" }
            var unlockCode = _config["Premium:UnlockBarcode"];
            if (!string.IsNullOrWhiteSpace(unlockCode) && barcodeRaw == unlockCode.Trim())
            {
                return true;
            }

            // ✅ REAL SYSTEM: hashed barcode stored in DB
            var hash = HashBarcode(barcodeRaw);

            return await _db.PremiumMemberships
                .AsNoTracking()
                .AnyAsync(x =>
                    x.StudentId == studentId &&
                    x.IsPurchased &&
                    x.BarcodeHash == hash,
                    ct);
        }

        // ✅ Securely hash barcode with salt
        public string HashBarcode(string barcodeRaw)
        {
            barcodeRaw = barcodeRaw.Trim();

            var salt = _config["Premium:BarcodeSalt"] ?? string.Empty;
            var input = $"{barcodeRaw}::{salt}";

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToHexString(bytes); // uppercase hex
        }
    }
}
