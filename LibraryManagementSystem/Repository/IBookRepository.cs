using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Repository;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBookAsync(CancellationToken cancellationToken);
    Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken);
    Task<Book> AddBookAsync(Book book, CancellationToken cancellationToken);
    Task<Book> UpdateBookAsync(Book book, CancellationToken cancellationToken);
    Task<Book> DeleteBooktAsync(int id, CancellationToken cancellationToken);
    IEnumerable<SelectListItem> Dropdown();
}
