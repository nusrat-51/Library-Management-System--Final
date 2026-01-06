using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers;

public class BookCategoryController : Controller
{
    private readonly IBookCategoryRepository _bookCategoryRepository;
    public BookCategoryController(IBookCategoryRepository bookCategoryRepository)
    {
        _bookCategoryRepository = bookCategoryRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _bookCategoryRepository.GetAllBookCategoryAsync(cancellationToken);
        return View(data);
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(int id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View(new BookCategory());
        }
        else
        {
            var data = await _bookCategoryRepository.GetBookCategoryByIdAsync(id, cancellationToken);
            if (data != null)
            {
                return View(data);
            }
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(BookCategory bookCategory, CancellationToken cancellationToken)
    {
        if (bookCategory.Id == 0)
        {
          
            await _bookCategoryRepository.AddBookCategoryAsync(bookCategory, cancellationToken);
            return RedirectToAction("Index");
        }
        else
        {
            await _bookCategoryRepository.UpdateBookCategoryAsync(bookCategory, cancellationToken);
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var data = await _bookCategoryRepository.GetBookCategoryByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _bookCategoryRepository.DeleteBookCategoryAsync(id, cancellationToken);
        return RedirectToAction("Index");
    }
}
