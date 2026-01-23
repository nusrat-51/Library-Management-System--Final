using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[Authorize]
public class BookApplicationController : Controller
{
    private readonly IBookApplicationRepository _bookApplicationRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ISignInHelper _signInHelper;

    private const string PremiumSessionKey = PremiumHandler.PremiumSessionKey;
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

    // ======================================================
    // DYNAMIC FINE CALCULATION
    // ======================================================
    private static void ApplyDynamicFine(IEnumerable<BookApplication> list)
    {
        foreach (var item in list)
        {
            if (item.Status == "Approved"
                && item.ReturnDate != default
                && item.ReturnDate.Date < DateTime.Today)
            {
                var days = (DateTime.Today - item.ReturnDate.Date).Days;
                item.FineAmount = days > 0 ? days * 10 : 0;
            }
            else
            {
                item.FineAmount = 0;
            }
        }
    }

    private string GetSafeStudentCardNo(long studentId)
    {
        var card = HttpContext.Session.GetString(StudentCardSessionKey);
        return string.IsNullOrWhiteSpace(card) ? studentId.ToString() : card;
    }

    // ======================================================
    // INDEX
    // ======================================================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var data = await _bookApplicationRepository.GetAllBookApplicationAsync(ct);

        if (User.IsInRole("Student"))
        {
            var sid = _signInHelper.UserId ?? 0;
            data = data.Where(x => x.StudentId == sid);
        }

        ApplyDynamicFine(data);
        return View(data);
    }

    // ======================================================
    // APPLY BOOK (FIXES 404)
    // URL: /BookApplication/ApplyBook/{bookId}
    // ======================================================
    [Authorize(Roles = "Student")]
    [HttpGet]
    public async Task<IActionResult> ApplyBook(int id, CancellationToken ct)
    {
        // ✅ FIX: use existing repository method
        var book = await _bookRepository.GetBookByIdAsync(id, ct);
        if (book == null) return NotFound();

        // ✅ PREMIUM CHECK
        if (book.IsPremium)
        {
            var unlocked = HttpContext.Session.GetString(PremiumSessionKey) == "1";
            if (!unlocked)
            {
                TempData["Error"] = "Premium membership required to apply for this book.";
                return RedirectToAction("Membership", "Premium");
            }
        }

        // ✅ Continue existing workflow
        return RedirectToAction(nameof(CreateOrEdit), new { id = 0 });
    }

    // ======================================================
    // CREATE OR EDIT (GET)
    // ======================================================
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(int id, CancellationToken ct)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        if (id == 0)
            return View(new BookApplication());

        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, ct);
        if (data == null) return NotFound();

        return View(data);
    }

    // ======================================================
    // CREATE OR EDIT (POST)
    // ======================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(BookApplication model, CancellationToken ct)
    {
        ViewData["BookId"] = _bookRepository.Dropdown();

        var studentId = _signInHelper.UserId ?? 0;

        if (model.Id == 0)
        {
            model.StudentId = studentId;
            model.StudentEmail = User?.Identity?.Name ?? "";
            model.StudentIdCardNo = GetSafeStudentCardNo(studentId);
            model.Status = "Approved"; // ✅ AUTO APPROVE
            model.CreatedAt = DateTime.UtcNow;

            await _bookApplicationRepository.AddBookApplicationAsync(model, ct);
        }
        else
        {
            await _bookApplicationRepository.UpdateBookApplicationAsync(model, ct);
        }

        return RedirectToAction(nameof(Index));
    }

    // ======================================================
    // DETAILS
    // ======================================================
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var data = await _bookApplicationRepository.GetBookApplicationByIdAsync(id, ct);
        if (data == null) return NotFound();
        return View(data);
    }

    // ======================================================
    // DELETE
    // ======================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    // ✅ FIX: Support all role naming styles used in your project (same as PaymentHistoryController idea)
    [Authorize(Roles = "Admin,Administrator,Manager,Mangement,Management")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _bookApplicationRepository.DeleteBookApplicationAsync(id, ct);
        return RedirectToAction(nameof(Index));
    }
}
