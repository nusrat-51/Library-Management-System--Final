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

        // ✅ keep same key name as BookApplicationController uses
        private const string StudentCardSessionKey = "StudentIdCardNo";

        public PremiumController(
            IPremiumAccessService premiumService,
            ISignInHelper signInHelper,
            ApplicationDbContext context)
        {
            _premiumService = premiumService;
            _signInHelper = signInHelper;
            _context = context;
        }

        // ✅ Premium Collection page: everyone can SEE, only unlocked can APPLY
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var premiumBooks = await _context.Books
                .AsNoTracking()
                .Where(b => b.IsPremium)
                .OrderByDescending(b => b.Id)
                .ToListAsync(ct);

            ViewData["Title"] = "Premium Collection";
            ViewData["ActivePage"] = "Premium";

            var studentId = _signInHelper.UserId ?? 0;
            bool canApplyPremium = false;

            if (studentId > 0)
            {
                // ✅ session unlock
                var unlocked = HttpContext.Session.GetString(PremiumHandler.PremiumSessionKey) == "1";

                // ✅ DB purchased membership unlock
                var purchased = await _premiumService.HasPurchasedMembershipAsync(studentId, ct);

                canApplyPremium = unlocked || purchased;

                // ✅ If purchased but session key not set, keep UI consistent (optional but helpful)
                if (purchased && !unlocked)
                {
                    HttpContext.Session.SetString(PremiumHandler.PremiumSessionKey, "1");
                }
            }

            ViewBag.CanApplyPremium = canApplyPremium;

            return View(premiumBooks);
        }

        [HttpGet]
        public IActionResult Unlock()
        {
            ViewData["Title"] = "Unlock Premium";
            ViewData["ActivePage"] = "Premium";
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
                return RedirectToAction(nameof(Unlock));
            }

            var ok = await _premiumService.ValidateBarcodeAsync(studentId, barcode, ct);
            if (!ok)
            {
                TempData["Error"] = "Invalid barcode. Please try again.";
                return RedirectToAction(nameof(Unlock));
            }

            // ✅ set session unlock (IMPORTANT)
            HttpContext.Session.SetString(PremiumHandler.PremiumSessionKey, "1");

            // ✅ ALSO store barcode/card no so StudentIdCardNo never becomes NULL in BookApplication
            HttpContext.Session.SetString(StudentCardSessionKey, barcode.Trim());

            TempData["Success"] = "Premium section unlocked successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Lock()
        {
            // ✅ remove both
            HttpContext.Session.Remove(PremiumHandler.PremiumSessionKey);
            HttpContext.Session.Remove(StudentCardSessionKey);

            TempData["Success"] = "Premium section locked.";
            return RedirectToAction(nameof(Unlock));
        }
    }
}
