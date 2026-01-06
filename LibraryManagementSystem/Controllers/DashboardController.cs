using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Dashboard";
            ViewData["ActivePage"] = "Dashboard";

            //  real DB values
            ViewBag.TotalBooks = await _context.Books.CountAsync(cancellationToken);
            ViewBag.ActiveMembers = await _context.Users.CountAsync(cancellationToken);

            ViewBag.BooksOnLoan = await _context.bookApplications
                .CountAsync(x => x.Status == "Approved" && x.ReturnDate >= DateTime.Today, cancellationToken);

            ViewBag.OverdueBooks = await _context.bookApplications
                .CountAsync(x => x.Status == "Approved" && x.ReturnDate < DateTime.Today, cancellationToken);

            //  Recent Book Loans (real)
            var recentLoans = await _context.bookApplications
                .Include(x => x.Book)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new
                {
                    BookTitle = x.Book != null ? x.Book.Title : "N/A",
                    Member = x.StudentEmail,
                    DueDate = x.ReturnDate,
                    Status = x.Status,
                    ReturnDate = x.ReturnDate
                })
                .ToListAsync(cancellationToken);

            var today = DateTime.Today;
            var displayLoans = recentLoans.Select(x =>
            {
                string displayStatus;

                if (x.Status == "Approved")
                {
                    if (x.ReturnDate < today) displayStatus = "Overdue";
                    else if (x.ReturnDate <= today.AddDays(3)) displayStatus = "Due Soon";
                    else displayStatus = "On Time";
                }
                else
                {
                    displayStatus = x.Status;
                }

                return new
                {
                    x.BookTitle,
                    x.Member,
                    x.DueDate,
                    DisplayStatus = displayStatus
                };
            }).ToList();

            ViewBag.RecentLoans = displayLoans;

            ViewBag.RecentBooks = await _context.Books
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .Select(b => new
                {
                    b.Title,
                    b.Author,
                    b.Category,
                    b.CreatedAt
                })
                .ToListAsync(cancellationToken);


            // ==========================================================
            // ✅ ADDED PART: STUDENT DASHBOARD COUNTS (Real-time DB values)
            // ==========================================================
            var email = User.Identity?.Name;

            if (!string.IsNullOrEmpty(email) && User.IsInRole("Student"))
            {
                // Total Borrowed (Approved)
                ViewBag.StudentTotalBorrowed = await _context.bookApplications
                    .CountAsync(x => x.StudentEmail == email && x.Status == "Approved", cancellationToken);

                // Currently Held = Approved AND return date today or future
                ViewBag.StudentCurrentlyHeld = await _context.bookApplications
                    .CountAsync(x => x.StudentEmail == email
                                  && x.Status == "Approved"
                                  && x.ReturnDate >= DateTime.Today, cancellationToken);

                // Overdue = Approved AND return date passed
                ViewBag.StudentOverdue = await _context.bookApplications
                    .CountAsync(x => x.StudentEmail == email
                                  && x.Status == "Approved"
                                  && x.ReturnDate < DateTime.Today, cancellationToken);

                // Pending Requests
                ViewBag.StudentPending = await _context.bookApplications
                    .CountAsync(x => x.StudentEmail == email && x.Status == "Pending", cancellationToken);
            }
            else
            {
                // fallback (student view won't break)
                ViewBag.StudentTotalBorrowed = 0;
                ViewBag.StudentCurrentlyHeld = 0;
                ViewBag.StudentOverdue = 0;
                ViewBag.StudentPending = 0;
            }
            // ==========================================================


            return View();
        }
    }
}
