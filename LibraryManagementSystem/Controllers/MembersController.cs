using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            // If you want: show all users (Members)
            var members = await _context.Users
                .OrderByDescending(x => x.RegisterDate)
                .ToListAsync(cancellationToken);

            // ✅ ADDED (does not affect existing logic)
            // Used by Views/Members/Index.cshtml to show Premium member count badge (Purchased only)
            ViewBag.PremiumCount = await _context.PremiumMemberships
                .CountAsync(x => x.IsPurchased, cancellationToken);

            ViewData["Title"] = "Members";
            ViewData["ActivePage"] = "Members";

            return View(members);
        }
    }
}
