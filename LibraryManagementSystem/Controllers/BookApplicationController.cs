using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Service;          // ✅ PremiumHandler
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[Authorize] // ✅ all actions require login
public class BookApplicationController : Controller
{
    private readonly IBookApplicationRepository _bookApplicationRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ISignInHelper _signInHelper;

    // ✅ Use SAME session key everywhere (PremiumHandler constant)
    private const string PremiumSessionKey = PremiumHandler.PremiumSessionKey;

    // ✅ store student card no in session (set this in PremiumController Unlock)
    private const string StudentCardSessionKey = "StudentIdCardNo";

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

    // ✅ Helper: StudentIdCardNo must NEVER be null/empty (DB constraint)
    private string GetSafeStudentCardNo(long studentId)
    {
        var cardNo = (HttpContext.Session.GetString(StudentCardSessionKey) ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(cardNo)) return cardNo;

        // fallback (so DB never gets NULL)
        return studentId.ToString();
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _bookApplicationRepository.GetAllBookApplicationAsync(cancellationToken);

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

        if (!ModelState.IsValid)
            return View(bookApplication);

        var studentId = _signInHelper.UserId ?? 0;

        if (User.IsInRole("Student"))
        {
            if (studentId <= 0) return Forbid();
            bookApplication.StudentId = studentId;
            bookApplication.StudentEmail = User?.Identity?.Name ?? bookApplication.StudentEmail;

            // ✅ ensure StudentIdCardNo
            bookApplication.StudentIdCardNo = (bookApplication.StudentIdCardNo ?? "").Trim();
            if (string.IsNullOrWhiteSpace(bookApplication.StudentIdCardNo))
                bookApplication.StudentIdCardNo = GetSafeStudentCardNo(studentId);
        }

        if (bookApplication.Id == 0)
        {
            bookApplication.Id = 0;
            bookApplication.Status = "Pending";

            if (bookApplication.CreatedAt == default)
                bookApplication.CreatedAt = DateTime.UtcNow;

            if (!User.IsInRole("Student") && bookApplication.StudentId <= 0)
                bookApplication.StudentId = 1;

            // ✅ ensure StudentIdCardNo (non-student too)
            bookApplication.StudentIdCardNo = (bookApplication.StudentIdCardNo ?? "").Trim();
            if (string.IsNullOrWhiteSpace(bookApplication.StudentIdCardNo))
                bookApplication.StudentIdCardNo = GetSafeStudentCardNo(bookApplication.StudentId);

            await _bookApplicationRepository.AddBookApplicationAsync(bookApplication, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        if (User.IsInRole("Student"))
        {
            var old = await _bookApplicationRepository.GetBookApplicationByIdAsync(bookApplication.Id, cancellationToken);
            if (old == null) return NotFound();
            if (old.StudentId != studentId) return Forbid();
        }

        // ✅ ensure StudentIdCardNo never null
        bookApplication.StudentIdCardNo = (bookApplication.StudentIdCardNo ?? "").Trim();
        if (string.IsNullOrWhiteSpace(bookApplication.StudentIdCardNo))
            bookApplication.StudentIdCardNo = GetSafeStudentCardNo(bookApplication.StudentId);

        await _bookApplicationRepository.UpdateBookApplicationAsync(bookApplication, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, cancellationToken);
        if (data == null) return NotFound();

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
        if (User.IsInRole("Student"))
            return Forbid();

        await _bookApplicationRepository.DeleteBookApplicationAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ✅ Applybook GET (manual form)
    [HttpGet]
    public async Task<IActionResult> Applybook(int id, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        var model = new BookApplication
        {
            StudentEmail = User?.Identity?.Name ?? "",
            IssueDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(7)
        };

        if (id > 0)
        {
            var book = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (book == null) return NotFound();

            // ✅ Premium protection (GET)
            if (book.IsPremium == true)
            {
                var isUnlocked = HttpContext.Session.GetString(PremiumSessionKey) == "1";
                if (!isUnlocked)
                {
                    TempData["Error"] = "This is a Premium book. Please unlock Premium using your barcode first.";
                    return RedirectToAction("Unlock", "Premium");
                }
            }

            model.BookId = book.Id;
            model.StudentId = _signInHelper.UserId ?? 0;

            // ✅ autofill card number (so form submit won't be NULL)
            var sid = model.StudentId;
            model.StudentIdCardNo = sid > 0 ? GetSafeStudentCardNo(sid) : "";
        }

        return View(model);
    }

    // ✅ Applybook POST (manual submit)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Applybook(BookApplication model, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        var studentId = _signInHelper.UserId ?? 0;
        if (studentId <= 0) return Forbid();

        model.Id = 0;
        model.StudentId = studentId;
        model.StudentEmail = User?.Identity?.Name ?? model.StudentEmail;
        model.Status = "Pending";

        if (model.CreatedAt == default)
            model.CreatedAt = DateTime.UtcNow;

        if (model.BookId <= 0)
        {
            ModelState.AddModelError("", "Please select a valid book.");
            return View(model);
        }

        // ✅ Premium protection (POST)
        var book = await _bookRepository.GetBookByIdAsync(model.BookId, cancellationToken);
        if (book == null) return NotFound();

        if (book.IsPremium == true)
        {
            var isUnlocked = HttpContext.Session.GetString(PremiumSessionKey) == "1";
            if (!isUnlocked)
            {
                TempData["Error"] = "This is a Premium book. Please unlock Premium using your barcode first.";
                return RedirectToAction("Unlock", "Premium");
            }
        }

        // ✅ IMPORTANT: StudentIdCardNo cannot be NULL in DB
        model.StudentIdCardNo = (model.StudentIdCardNo ?? "").Trim();
        if (string.IsNullOrWhiteSpace(model.StudentIdCardNo))
            model.StudentIdCardNo = GetSafeStudentCardNo(studentId);

        await _bookApplicationRepository.AddBookApplicationAsync(model, cancellationToken);

        TempData["Success"] = "Book application submitted successfully!";
        return RedirectToAction(nameof(Index));
    }

    // ✅ Premium Collection Apply button
    // ✅ FIX: Do NOT auto submit. Just redirect to Applybook manual form.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> ApplyPremium(int bookId, CancellationToken cancellationToken)
    {
        var studentId = _signInHelper.UserId ?? 0;
        if (studentId <= 0) return Forbid();

        var isUnlocked = HttpContext.Session.GetString(PremiumSessionKey) == "1";
        if (!isUnlocked)
        {
            TempData["Error"] = "Please unlock Premium using your barcode first.";
            return RedirectToAction("Unlock", "Premium");
        }

        var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
        if (book == null) return NotFound();

        if (book.IsPremium != true)
        {
            TempData["Error"] = "This book is not a Premium book.";
            return RedirectToAction("Index", "Premium");
        }

        // ✅ Redirect to manual apply form (no auto insert)
        return RedirectToAction("Applybook", "BookApplication", new { id = bookId });
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
