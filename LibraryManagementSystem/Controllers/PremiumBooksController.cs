using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // ✅ helper: load category dropdown (NO "Name" dependency)
        private async Task LoadCategoriesAsync(CancellationToken ct)
        {
            var cats = await _context.BookCategories
                .AsNoTracking()
                .ToListAsync(ct);

            // ✅ build dropdown safely (works even if Name doesn't exist)
            var items = cats
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text =
                        (c.GetType().GetProperty("CategoryName")?.GetValue(c)?.ToString()) ??
                        (c.GetType().GetProperty("Name")?.GetValue(c)?.ToString()) ??
                        (c.GetType().GetProperty("Category")?.GetValue(c)?.ToString()) ??
                        (c.GetType().GetProperty("Title")?.GetValue(c)?.ToString()) ??
                        $"Category #{c.Id}"
                })
                .OrderBy(x => x.Text)
                .ToList();

            ViewBag.CategoryId = items;
        }

        // ================= INDEX =================
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

        // ================= CREATE =================
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            ViewData["Title"] = "Add Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            await LoadCategoriesAsync(ct);
            return View(new Book());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, CancellationToken ct)
        {
            ViewData["Title"] = "Add Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            await LoadCategoriesAsync(ct);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.IsPremium = true;

                if (model.AvailableCopies > model.TotalCopies)
                    model.AvailableCopies = model.TotalCopies;

                _context.Books.Add(model);
                await _context.SaveChangesAsync(ct);

                TempData["Success"] = "Premium book added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Category selection is invalid. Please select a valid category from dropdown.";
                return View(model);
            }
        }

        // ================= EDIT =================
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            ViewData["Title"] = "Edit Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            var book = await _context.Books
                .FirstOrDefaultAsync(x => x.Id == id && x.IsPremium, ct);

            if (book == null) return NotFound();

            await LoadCategoriesAsync(ct);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model, CancellationToken ct)
        {
            ViewData["Title"] = "Edit Premium Book";
            ViewData["ActivePage"] = "PremiumBooks";

            await LoadCategoriesAsync(ct);

            if (!ModelState.IsValid)
                return View(model);

            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == model.Id, ct);
            if (book == null) return NotFound();

            try
            {
                book.IsPremium = true;
                book.Title = model.Title;
                book.Author = model.Author;

                // ✅ IMPORTANT: FK safe save (CategoryId must be from dropdown)
                book.CategoryId = model.CategoryId;

                book.TotalCopies = model.TotalCopies;
                book.AvailableCopies = model.AvailableCopies > model.TotalCopies
                    ? model.TotalCopies
                    : model.AvailableCopies;

                await _context.SaveChangesAsync(ct);

                TempData["Success"] = "Premium book updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Category selection is invalid. Please select a valid category from dropdown.";
                return View(model);
            }
        }

        // ================= REMOVE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Administrator,Manager,Management,Mangement")]
        public async Task<IActionResult> Remove(int id, CancellationToken ct)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (book == null) return NotFound();

            book.IsPremium = false;
            await _context.SaveChangesAsync(ct);

            TempData["Success"] = "Book removed from Premium Collection.";
            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE (ALIAS) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Administrator,Manager,Management,Mangement")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            return await Remove(id, ct);
        }
    }
}
