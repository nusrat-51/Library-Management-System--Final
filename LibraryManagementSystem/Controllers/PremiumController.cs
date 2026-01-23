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

        // ✅ New: separate key for storing barcode (so we don't mix "student card no" with "barcode")
        private const string PremiumBarcodeSessionKey = "PremiumBarcode";

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
                // ✅ 1) Must unlock using barcode (session unlock)
                var unlocked = HttpContext.Session.GetString(PremiumHandler.PremiumSessionKey) == "1";

                // ✅ 2) Must have ACTIVE membership in DB (paid + not expired)
                // IMPORTANT: service must return true only when Active & not expired
                var purchased = await _premiumService.HasPurchasedMembershipAsync(studentId, ct);

                // ✅ YOUR RULE: Apply only if BOTH are true
                canApplyPremium = unlocked && purchased;

                // ❌ REMOVED AUTO-SESSION SET (we keep logic, but we must NOT unlock session automatically)
                // because you said barcode unlock is required after buying membership.
                // if (purchased && !unlocked)
                // {
                //     HttpContext.Session.SetString(PremiumHandler.PremiumSessionKey, "1");
                // }
            }

            ViewBag.CanApplyPremium = canApplyPremium;

            return View(premiumBooks);
        }

        // ✅ Premium Membership purchase page (student will buy online)
        // This does NOT remove barcode unlock. It is an additional feature.
        [HttpGet]
        public async Task<IActionResult> Membership(CancellationToken ct)
        {
            ViewData["Title"] = "Premium Membership";
            ViewData["ActivePage"] = "PremiumMembership";

            var studentId = _signInHelper.UserId ?? 0;
            if (studentId <= 0) return Forbid();

            // ✅ Keep your existing logic (do not delete)
            var hasMembership = await _premiumService.HasPurchasedMembershipAsync(studentId, ct);

            // ✅ Improve UI accuracy using DB fields (Active + not expired)
            var activeMembership = await _context.PremiumMemberships
                .AsNoTracking()
                .Where(x => x.StudentId == studentId
                            && x.Status == "Active"
                            && x.EndDate != null
                            && x.EndDate.Value.Date >= DateTime.Today)
                .OrderByDescending(x => x.EndDate)
                .FirstOrDefaultAsync(ct);

            bool hasActiveMembership = activeMembership != null;
            ViewBag.HasMembership = hasActiveMembership || hasMembership;

            // ✅ Provide EndDate for the "Valid until" badge in Membership.cshtml
            ViewBag.EndDate = activeMembership?.EndDate;

            // ❌ DO NOT auto-unlock premium session here
            // because your rule says student must unlock with barcode after buying membership.
            // (We keep this comment so you see the reason clearly.)

            return View();
        }

        // ✅ Student clicks buy -> we redirect to PaymentController flow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuyMembership(int days = 30)
        {
            var studentId = _signInHelper.UserId ?? 0;
            if (studentId <= 0) return Forbid();

            return RedirectToAction("MembershipCheckout", "Payment", new { days });
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

            // ✅ Keep your existing logic (don't delete):
            HttpContext.Session.SetString(StudentCardSessionKey, barcode.Trim());
            HttpContext.Session.SetString(PremiumBarcodeSessionKey, barcode.Trim());

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
            HttpContext.Session.Remove(PremiumBarcodeSessionKey);

            TempData["Success"] = "Premium section locked.";
            return RedirectToAction(nameof(Unlock));
        }
    }
}
