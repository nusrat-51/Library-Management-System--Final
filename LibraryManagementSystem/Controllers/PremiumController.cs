using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class PremiumController : Controller
    {
        private readonly IPremiumAccessService _premiumService;
        private readonly ISignInHelper _signInHelper;
        private readonly ApplicationDbContext _context;

        public PremiumController(
            IPremiumAccessService premiumService,
            ISignInHelper signInHelper,
            ApplicationDbContext context)
        {
            _premiumService = premiumService;
            _signInHelper = signInHelper;
            _context = context;
        }

        // ✅ Only unlocked/premium students can see this
        [Authorize(Policy = "PremiumOnly")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var premiumBooks = await _context.Books
                .AsNoTracking()
                .Where(b => b.IsPremium)
                .OrderByDescending(b => b.Id)
                .ToListAsync(ct);

            ViewData["Title"] = "Premium Collection";
            ViewData["ActivePage"] = "Premium";

            return View(premiumBooks);
        }

        [HttpGet]
        public IActionResult Unlock()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string barcode, CancellationToken ct)
        {
            var studentId = _signInHelper.UserId ?? 0;
            if (studentId <= 0) return Forbid();

            if (string.IsNullOrWhiteSpace(barcode))
            {
                TempData["Error"] = "Please enter your barcode.";
                return RedirectToAction("Unlock");
            }

            var ok = await _premiumService.ValidateBarcodeAsync(studentId, barcode, ct);
            if (!ok)
            {
                TempData["Error"] = "Invalid barcode. Please try again.";
                return RedirectToAction("Unlock");
            }

            HttpContext.Session.SetString(PremiumHandler.PremiumSessionKey, "1");
            TempData["Success"] = "Premium section unlocked successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Lock()
        {
            HttpContext.Session.Remove(PremiumHandler.PremiumSessionKey);
            TempData["Success"] = "Premium section locked.";
            return RedirectToAction("Unlock");
        }
    }
}
