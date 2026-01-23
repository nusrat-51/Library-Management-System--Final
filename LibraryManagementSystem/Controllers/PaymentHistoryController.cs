using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    // ✅ FIX: Support all role naming styles used in your project
    [Authorize(Roles = "Admin,Administrator,Manager,Mangement,Management,Librarian")]
    public class PaymentHistoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaymentHistoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewData["Title"] = "Payment History";
            ViewData["ActivePage"] = "PaymentHistory";

            var finePayments = await _db.FinePayments
                .AsNoTracking()
                .OrderByDescending(x => x.PaidAt ?? x.CreatedAt)
                .ToListAsync(ct);

            var memberships = await _db.PremiumMemberships
                .AsNoTracking()
                .OrderByDescending(x => x.PaidAt ?? x.PurchasedAt ?? x.CreatedAt)
                .ToListAsync(ct);

            ViewBag.FinePayments = finePayments;
            ViewBag.Memberships = memberships;

            return View();
        }
    }
}
