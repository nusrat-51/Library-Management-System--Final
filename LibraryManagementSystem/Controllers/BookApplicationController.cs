using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[Authorize] // ✅ all actions require login (matches your role-based system)
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

    // ✅ Helper: calculate fine dynamically (NO DB save here, only for UI)
    // ✅ Update: Fine applies only to Approved (issued) applications
    private static void ApplyDynamicFine(IEnumerable<BookApplication> list, decimal finePerDay)
    {
        foreach (var item in list)
        {
            // ✅ Only approved/issued can have fine
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

        // ✅ Student must see ONLY own applications
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            data = data.Where(x => x.StudentId == studentId);
        }

        // ✅ Calculate fine dynamically (UI only)
        decimal finePerDay = 10m;
        ApplyDynamicFine(data, finePerDay);

        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(int id, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        if (id == 0)
        {
            return View(new BookApplication());
        }

        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, cancellationToken);
        if (data != null)
        {
            // ✅ Block students from editing others by URL
            if (User.IsInRole("Student"))
            {
                var studentId = _signInHelper.UserId ?? 0;
                if (data.StudentId != studentId)
                    return Forbid();
            }

            return View(data);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // ✅ good practice (won’t break)
    public async Task<IActionResult> CreateOrEdit(BookApplication bookApplication, CancellationToken cancellationToken)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        // ✅ Student can only create/update for themselves (prevent tampering)
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            bookApplication.StudentId = studentId;
        }

        if (bookApplication.Id == 0)
        {
            // keep your original behavior
            bookApplication.StudentId = _signInHelper.UserId ?? 1;
            bookApplication.Status = "Pending";

            await _bookApplicationRepository.AddBookApplicationAsync(bookApplication, cancellationToken);
            return RedirectToAction("Index");
        }

        // ✅ Block students from updating others by URL/post
        if (User.IsInRole("Student"))
        {
            var old = await _bookApplicationRepository.GetBookApplicationByIdAsync(bookApplication.Id, cancellationToken);
            var studentId = _signInHelper.UserId ?? 0;

            if (old == null) return NotFound();
            if (old.StudentId != studentId) return Forbid();
        }

        await _bookApplicationRepository.UpdateBookApplicationAsync(bookApplication, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, cancellationToken);
        if (data == null) return NotFound();

        // ✅ Block students from opening others' details by URL
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            if (data.StudentId != studentId)
            {
                return Forbid(); // or return NotFound();
            }
        }

        // ✅ Keep UI consistent: calculate fine for details view too
        decimal finePerDay = 10m;
        ApplyDynamicFine(new[] { data }, finePerDay);

        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // ✅ safe (requires you to add token in Delete form if not already)
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        // ✅ Block students from deleting by URL
        if (User.IsInRole("Student"))
            return Forbid();

        await _bookApplicationRepository.DeleteBookApplicationAsync(id, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Applybook(int id, CancellationToken cancellationToken)
    {
        var bookApplication = new BookApplication();
        ViewData["BookId"] = _bookRepository.Dropdown();

        if (id > 0)
        {
            var data = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            bookApplication.BookId = data.Id;
            bookApplication.StudentId = _signInHelper.UserId ?? 1;
        }

        return View(bookApplication);
    }

    [HttpGet]
    public async Task<IActionResult> Recent(string? status, bool includePending = false, CancellationToken cancellationToken = default)
    {
        var data = await _bookApplicationRepository.GetAllBookApplicationAsync(cancellationToken);

        // If student visits Recent, filter to own data
        if (User.IsInRole("Student"))
        {
            var studentId = _signInHelper.UserId ?? 0;
            data = data.Where(x => x.StudentId == studentId);
        }

        // Filter: Approved only OR Approved + Pending
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (includePending && status == "Approved")
            {
                data = data.Where(x => x.Status == "Approved" || x.Status == "Pending");
            }
            else
            {
                data = data.Where(x => x.Status == status);
            }
        }

        // Sort newest first (real)
        data = data.OrderByDescending(x => x.CreatedAt);

        // Calculate fine dynamically here too
        decimal finePerDay = 10m;
        ApplyDynamicFine(data, finePerDay);

        // Reuse same Index view
        return View("Index", data);
    }
}
