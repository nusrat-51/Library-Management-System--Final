using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[Authorize] // ✅ all actions require login
public class BookApplicationController : Controller
{
    private readonly IBookApplicationRepository _bookApplicationRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ISignInHelper _signInHelper;

    public BookApplicationController(
        IBookApplicationRepository bookApplicationRepository,
        IBookRepository bookRepository,
        ISignInHelper signInHelper)
    {
        _bookApplicationRepository = bookApplicationRepository;
        _bookRepository = bookRepository;
        _signInHelper = signInHelper;
    }

    // ✅ Helper: calculate fine dynamically (UI only)
    // ✅ Fine applies only to Approved (issued) applications
    private static void ApplyDynamicFine(IEnumerable<BookApplication> list, decimal finePerDay)
    {
        foreach (var item in list)
        {
            if ((item.Status ?? "") == "Approved"
                && item.ReturnDate != default
                && item.ReturnDate.Date < DateTime.Today)
            {
                var overdueDays = (DateTime.Today - item.ReturnDate.Date).Days;
                item.FineAmount = overdueDays > 0 ? overdueDays * finePerDay : 0;
            }
            else
            {
                item.FineAmount = 0;
            }
        }
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _bookApplicationRepository.GetAllBookApplicationAsync(cancellationToken);

        // ✅ Student sees ONLY own applications
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            data = data.Where(x => x.StudentId == studentId);
        }

        decimal finePerDay = 10m;
        ApplyDynamicFine(data, finePerDay);

        return View(data);
    }

    // ✅ CreateOrEdit GET
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(int id, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        if (id == 0)
            return View(new BookApplication());

        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, cancellationToken);
        if (data == null) return NotFound();

        // ✅ Block students from editing others
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            if (data.StudentId != studentId) return Forbid();
        }

        return View(data);
    }

    // ✅ CreateOrEdit POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(BookApplication bookApplication, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        // ✅ If validation fails, show errors
        if (!ModelState.IsValid)
            return View(bookApplication);

        var studentId = _signInHelper.UserId ?? 0;

        // ✅ Student can only create/update for themselves (prevent tampering)
        if (User.IsInRole("Student"))
        {
            if (studentId <= 0) return Forbid();
            bookApplication.StudentId = studentId;

            // ✅ protect email/id from tampering
            bookApplication.StudentEmail = User?.Identity?.Name ?? bookApplication.StudentEmail;
        }

        if (bookApplication.Id == 0)
        {
            // ✅ FIX: IDENTITY INSERT PROTECTION (MOST IMPORTANT)
            // DB auto-generate করবে, তাই 0 করে দাও
            bookApplication.Id = 0;

            // ✅ new request = Pending
            bookApplication.Status = "Pending";

            // ✅ ensure CreatedAt
            if (bookApplication.CreatedAt == default)
                bookApplication.CreatedAt = DateTime.UtcNow;

            // if non-student creates, ensure studentId exists
            if (!User.IsInRole("Student") && bookApplication.StudentId <= 0)
                bookApplication.StudentId = 1;

            await _bookApplicationRepository.AddBookApplicationAsync(bookApplication, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Block students from updating others
        if (User.IsInRole("Student"))
        {
            var old = await _bookApplicationRepository.GetBookApplicationByIdAsync(bookApplication.Id, cancellationToken);
            if (old == null) return NotFound();
            if (old.StudentId != studentId) return Forbid();
        }

        await _bookApplicationRepository.UpdateBookApplicationAsync(bookApplication, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, cancellationToken);
        if (data == null) return NotFound();

        // ✅ Block students from opening others
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            if (data.StudentId != studentId) return Forbid();
        }

        decimal finePerDay = 10m;
        ApplyDynamicFine(new[] { data }, finePerDay);

        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        // ✅ Block students from deleting
        if (User.IsInRole("Student"))
            return Forbid();

        await _bookApplicationRepository.DeleteBookApplicationAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ✅ Applybook GET (kept) + ✅ FIX: auto fill email/date + protect BookId
    [HttpGet]
    public async Task<IActionResult> Applybook(int id, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        var model = new BookApplication();

        // ✅ Auto fill student email if available (your login uses email as username)
        model.StudentEmail = User?.Identity?.Name ?? "";

        // ✅ default dates
        model.IssueDate = DateTime.Today;
        model.ReturnDate = DateTime.Today.AddDays(7);

        if (id > 0)
        {
            var book = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (book == null) return NotFound();

            model.BookId = book.Id;
            model.StudentId = _signInHelper.UserId ?? 0;
        }

        return View(model);
    }

    // ✅ Applybook POST (Save Book button কাজ করবে)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Applybook(BookApplication model, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        // ✅ validate login studentId
        var studentId = _signInHelper.UserId ?? 0;
        if (studentId <= 0) return Forbid();

        // ✅ FIX: IDENTITY INSERT PROTECTION (MOST IMPORTANT)
        model.Id = 0;

        // ✅ student can only apply for self
        model.StudentId = studentId;

        // ✅ protect email from tampering (always take logged in email)
        model.StudentEmail = User?.Identity?.Name ?? model.StudentEmail;

        // ✅ new request = Pending
        model.Status = "Pending";

        // ✅ ensure CreatedAt
        if (model.CreatedAt == default)
            model.CreatedAt = DateTime.UtcNow;

        // ✅ basic validation
        if (model.BookId <= 0)
        {
            ModelState.AddModelError("", "Please select a valid book.");
            return View(model);
        }

        await _bookApplicationRepository.AddBookApplicationAsync(model, cancellationToken);

        TempData["Success"] = "Book application submitted successfully!";
        return RedirectToAction(nameof(Index));
    }

    // ✅ Premium Collection Apply button (quick submit)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> ApplyPremium(int bookId, CancellationToken cancellationToken)
    {
        var studentId = _signInHelper.UserId ?? 0;
        if (studentId <= 0) return Forbid();

        var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
        if (book == null) return NotFound();

        var app = new BookApplication
        {
            // ✅ FIX: identity insert protection
            Id = 0,

            BookId = book.Id,
            StudentId = studentId,
            StudentEmail = User?.Identity?.Name ?? "",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            IssueDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(7)
        };

        await _bookApplicationRepository.AddBookApplicationAsync(app, cancellationToken);

        TempData["Success"] = "Application submitted successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Recent(string? status, bool includePending = false, CancellationToken cancellationToken = default)
    {
        var data = await _bookApplicationRepository.GetAllBookApplicationAsync(cancellationToken);

        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            data = data.Where(x => x.StudentId == studentId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (includePending && status == "Approved")
                data = data.Where(x => x.Status == "Approved" || x.Status == "Pending");
            else
                data = data.Where(x => x.Status == status);
        }

        data = data.OrderByDescending(x => x.CreatedAt);

        decimal finePerDay = 10m;
        ApplyDynamicFine(data, finePerDay);

        return View("Index", data);
    }
}
