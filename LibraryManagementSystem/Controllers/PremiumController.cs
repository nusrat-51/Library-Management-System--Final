using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class PremiumController : Controller
    {
        private readonly IPremiumAccessService _premiumService;
        private readonly ISignInHelper _signInHelper;

        public PremiumController(IPremiumAccessService premiumService, ISignInHelper signInHelper)
        {
            _premiumService = premiumService;
            _signInHelper = signInHelper;
        }

        // Premium Section (Locked)
        [Authorize(Policy = "PremiumOnly")]
        public IActionResult Index()
        {
            return View();
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

            var ok = await _premiumService.ValidateBarcodeAsync(studentId, barcode, ct);
            if (string.IsNullOrWhiteSpace(barcode))
            {
                TempData["Error"] = "Please enter your barcode.";
                return RedirectToAction("Unlock");
            }


            HttpContext.Session.SetString(PremiumHandler.PremiumSessionKey, "1");
            TempData["Success"] = "Premium section unlocked successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Lock()
        {
            HttpContext.Session.Remove(PremiumHandler.PremiumSessionKey);
            TempData["Success"] = "Premium section locked.";
            return RedirectToAction("Unlock");
        }
    }
}
