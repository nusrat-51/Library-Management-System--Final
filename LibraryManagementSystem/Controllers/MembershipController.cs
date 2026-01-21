using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    // ✅ Librarian/Admin/Manager can manage premium memberships
    [Authorize(Roles = "Librarian,Admin,Manager")]
    public class MembershipController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPremiumAccessService _premiumService;

        public MembershipController(ApplicationDbContext db, IPremiumAccessService premiumService)
        {
            _db = db;
            _premiumService = premiumService;
        }

        // ✅ List page
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var list = await _db.PremiumMemberships
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            return View(list);
        }

        // ✅ Create or Re-issue barcode (Not purchased yet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(long studentId, string studentName, string studentEmail, CancellationToken ct)
        {
            // ✅ basic validation
            if (studentId <= 0)
            {
                TempData["Error"] = "Student ID is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(studentEmail))
            {
                TempData["Error"] = "Student Email is required.";
                return RedirectToAction("Index");
            }

            studentEmail = studentEmail.Trim();
            studentName = studentName ?? "";

            // ✅ generate barcode (12 chars)
            var barcode = Guid.NewGuid().ToString("N")[..12].ToUpper();

            // ✅ IMPORTANT: Find by StudentId FIRST (most reliable), fallback to email
            var existing = await _db.PremiumMemberships
                .FirstOrDefaultAsync(x => x.StudentId == studentId, ct);

            if (existing == null)
            {
                existing = await _db.PremiumMemberships
                    .FirstOrDefaultAsync(x => x.StudentEmail == studentEmail, ct);
            }

            if (existing == null)
            {
                // ✅ create new
                existing = new PremiumMembership
                {
                    StudentId = studentId,
                    StudentName = studentName,
                    StudentEmail = studentEmail,
                    BarcodeHash = _premiumService.HashBarcode(barcode),
                    IsPurchased = false,
                    CreatedAt = DateTime.Now
                };

                _db.PremiumMemberships.Add(existing);
            }
            else
            {
                // ✅ update existing (reissue barcode)
                existing.StudentId = studentId;
                existing.StudentName = string.IsNullOrWhiteSpace(studentName) ? existing.StudentName : studentName;
                existing.StudentEmail = studentEmail;
                existing.BarcodeHash = _premiumService.HashBarcode(barcode);

                //  keep purchased status as-is (do not break old logic)
                if (!existing.IsPurchased)
                    existing.CreatedAt = DateTime.Now;
            }

            await _db.SaveChangesAsync(ct);

            // show barcode to librarian so they can write/print it
            TempData["Success"] = $"Barcode issued successfully. Barcode: {barcode}";
            return RedirectToAction("Index");
        }

        //  Mark purchased (after taking 100 taka)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPurchased(int id, CancellationToken ct)
        {
            var m = await _db.PremiumMemberships.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (m == null) return NotFound();

            m.IsPurchased = true;
            m.PurchasedAt = DateTime.Now;

            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Membership marked as Purchased (Paid).";
            return RedirectToAction("Index");
        }
    }
}
