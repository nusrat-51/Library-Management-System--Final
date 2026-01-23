using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    [Route("StudentPaymentHistory")]
    public class StudentPaymentHistoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StudentPaymentHistoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ This makes /StudentPaymentHistory work
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewData["Title"] = "My Payment History";
            ViewData["ActivePage"] = "StudentPaymentHistory";

            var email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.FindFirstValue("email")
                        ?? User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
                return Forbid();

            // ✅ reads SAME DB tables as manager dashboard
            var finePayments = await _db.FinePayments
                .AsNoTracking()
                .Where(x => x.StudentEmail == email)
                .OrderByDescending(x => x.PaidAt ?? x.CreatedAt)
                .ToListAsync(ct);

            var memberships = await _db.PremiumMemberships
                .AsNoTracking()
                .Where(x => x.StudentEmail == email)
                .OrderByDescending(x => x.PaidAt ?? x.PurchasedAt ?? x.CreatedAt)
                .ToListAsync(ct);

            ViewBag.FinePayments = finePayments;
            ViewBag.Memberships = memberships;

            return View();
        }
    }
}
