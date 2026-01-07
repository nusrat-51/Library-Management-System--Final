using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager,Librarian")]
    public class MembershipController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPremiumAccessService _premiumService;

        public MembershipController(ApplicationDbContext db, IPremiumAccessService premiumService)
        {
            _db = db;
            _premiumService = premiumService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var list = await _db.PremiumMemberships
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            return View(list);
        }

        // Issue barcode for a student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(long studentId, string studentEmail, CancellationToken ct)
        {
            // generate a simple barcode string
            var barcode = Guid.NewGuid().ToString("N")[..12].ToUpper(); // 12 chars

            var existing = await _db.PremiumMemberships
                .FirstOrDefaultAsync(x => x.StudentId == studentId, ct);

            if (existing == null)
            {
                existing = new PremiumMembership
                {
                    StudentId = studentId,
                    StudentEmail = studentEmail ?? "",
                    BarcodeHash = _premiumService.HashBarcode(barcode),
                    IsPurchased = false,
                    CreatedAt = DateTime.Now
                };

                _db.PremiumMemberships.Add(existing);
            }
            else
            {
                existing.StudentEmail = studentEmail ?? existing.StudentEmail;
                existing.BarcodeHash = _premiumService.HashBarcode(barcode);
            }

            await _db.SaveChangesAsync(ct);

            TempData["Success"] = $"Barcode issued successfully. Barcode: {barcode}";
            return RedirectToAction("Index");
        }

        // Mark purchased
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPurchased(int id, CancellationToken ct)
        {
            var m = await _db.PremiumMemberships.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (m == null) return NotFound();

            m.IsPurchased = true;
            m.PurchasedAt = DateTime.Now;

            await _db.SaveChangesAsync(ct);

            TempData["Success"] = "Membership marked as Purchased.";
            return RedirectToAction("Index");
        }
    }
}
