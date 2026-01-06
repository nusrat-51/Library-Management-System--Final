using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Repository;

public interface IBookApplicationRepository
{
    Task<IEnumerable<BookApplication>> GetAllBookApplicationAsync(CancellationToken cancellationToken);
    Task<BookApplication> GetBookApplicationByIdAsync(int id, CancellationToken cancellationToken);
    Task<BookApplication> AddBookApplicationAsync(BookApplication  bookApplication, CancellationToken cancellationToken);
    Task<BookApplication> UpdateBookApplicationAsync(BookApplication  bookApplication, CancellationToken cancellationToken);
    Task<BookApplication> DeleteBookApplicationAsync(int id, CancellationToken cancellationToken);



}
