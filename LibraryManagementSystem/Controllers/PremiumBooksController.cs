using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Administrator,Manager,Management,Mangement,Librarian")]
    public class PremiumBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremiumBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var books = await _context.Books
                .AsNoTracking()
                .Where(b => b.IsPremium)
                .OrderByDescending(b => b.Id)
                .ToListAsync(ct);

            ViewData["Title"] = "Premium Books";
            ViewData["ActivePage"] = "PremiumBooks";

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Add Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            return View(new Book
            {
                BookApplications = new List<BookApplication>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, CancellationToken ct)
        {
            ViewData["Title"] = "Add Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            model.BookApplications ??= new List<BookApplication>();

            // ✅ FK FIX (No BookCategory.Name dependency)
            // If CategoryId is not provided, pick any existing category from DB
            if (model.CategoryId <= 0)
            {
                var firstCategoryId = await _context.BookCategories
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync(ct);

                if (firstCategoryId <= 0)
                {
                    ModelState.AddModelError("", "No Book Category exists. Please add a category first.");
                    TempData["Error"] = "No Book Category exists. Please add a category first.";
                    return View(model);
                }

                model.CategoryId = firstCategoryId;
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors and try again.";
                return View(model);
            }

            model.IsPremium = true;

            if (model.AvailableCopies > model.TotalCopies)
                model.AvailableCopies = model.TotalCopies;

            _context.Books.Add(model);
            await _context.SaveChangesAsync(ct);

            TempData["Success"] = "Premium book added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id, CancellationToken ct)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (book == null) return NotFound();

            book.IsPremium = false;
            await _context.SaveChangesAsync(ct);

            TempData["Success"] = "Book removed from Premium Collection.";
            return RedirectToAction(nameof(Index));
        }
    }
}
