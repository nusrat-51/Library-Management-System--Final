using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Repository;

public interface IBookCategoryRepository
{
    Task<IEnumerable<BookCategory>> GetAllBookCategoryAsync(CancellationToken cancellationToken);
    Task<BookCategory> GetBookCategoryByIdAsync(int id, CancellationToken cancellationToken);
    Task<BookCategory> AddBookCategoryAsync(BookCategory  bookCategory,CancellationToken cancellationToken);
    Task<BookCategory> UpdateBookCategoryAsync(BookCategory  bookCategory,CancellationToken cancellationToken);
    Task<BookCategory> DeleteBookCategoryAsync(int id, CancellationToken cancellationToken);

    IEnumerable<SelectListItem> Dropdown();
}
