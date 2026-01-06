using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Normal page (HTML)
        [HttpGet]
        public async Task<IActionResult> Index(int? month, int? year, CancellationToken cancellationToken)
        {
            await LoadMonthlyReport(month, year, cancellationToken);
            return View();
        }

        // ✅ PDF/Print page (HTML formatted for printing / Save as PDF)
        // Open this in a new tab. User prints -> "Save as PDF".
        [HttpGet]
        public async Task<IActionResult> Pdf(int? month, int? year, CancellationToken cancellationToken)
        {
            await LoadMonthlyReport(month, year, cancellationToken);

            // Optional: remove layout for clean PDF output (handled in Pdf.cshtml with Layout = null)
            return View("Pdf");
        }

        // ✅ Shared logic (YOUR LOGIC reused - nothing deleted)
        private async Task LoadMonthlyReport(int? month, int? year, CancellationToken cancellationToken)
        {
            int selectedMonth = month ?? DateTime.Today.Month;
            int selectedYear = year ?? DateTime.Today.Year;

            var startDate = new DateTime(selectedYear, selectedMonth, 1);
            var endDate = startDate.AddMonths(1);

            // ✅ 1) Total issued books in month (same logic)
            var totalIssued = await _context.bookApplications
                .CountAsync(x => x.Status == "Approved"
                              && x.CreatedAt >= startDate
                              && x.CreatedAt < endDate, cancellationToken);

            // ✅ 2) Fine calculation (same logic)
            decimal finePerDay = 10;

            // ✅ FIX: Include Book to safely read Book.Title
            var monthlyApproved = await _context.bookApplications
                .Include(x => x.Book)
                .Where(x => x.Status == "Approved"
                         && x.CreatedAt >= startDate
                         && x.CreatedAt < endDate)
                .Select(x => new
                {
                    x.StudentEmail,
                    x.ReturnDate,
                    x.CreatedAt,
                    BookTitle = x.Book != null ? x.Book.Title : "N/A",
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            decimal totalFineDue = 0;

            foreach (var item in monthlyApproved)
            {
                if (item.ReturnDate < DateTime.Today)
                {
                    var overdueDays = (DateTime.Today - item.ReturnDate).Days;
                    totalFineDue += overdueDays * finePerDay;
                }
            }

            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.TotalIssued = totalIssued;
            ViewBag.TotalFineDue = totalFineDue;
            ViewBag.FinePerDay = finePerDay;
            ViewBag.MonthlyIssuedList = monthlyApproved;
        }
    }
}
