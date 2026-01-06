using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Book> AddBookAsync(Book book, CancellationToken cancellationToken)
    {
         await _context.Books.AddAsync(book, cancellationToken);
        if (book.CreatedAt == default)
            book.CreatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
         return book;
    }

    public async Task<Book> DeleteBooktAsync(int id, CancellationToken cancellationToken)
    {
       var data = await _context.Books.FindAsync(id);
         if (data != null)
         {
                _context.Books.Remove(data);
                await _context.SaveChangesAsync(cancellationToken);
                return data;
        }
         return null!;
    }

    public IEnumerable<SelectListItem> Dropdown()
    {
        var data = _context.Books.Select(x => new SelectListItem
        {
            Text = x.Title,
            Value = x.Id.ToString()
        }).ToList();
        return data;
    }

    public async Task<IEnumerable<Book>> GetAllBookAsync(CancellationToken cancellationToken)
    {
       var data = await _context.Books.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        return null;

    }

    public async Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.Books.FindAsync(id);
        if (data != null)
        {
            return data;
        }
        return null!;
    }

    public async Task<Book> UpdateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var data = await _context.Books.FindAsync(book.Id);
        if (data != null) 
        { 
            data.Title = book.Title;
            data.Author = book.Author;
            data.Category = book.Category;
            data.TotalCopies = book.TotalCopies;
            data.AvailableCopies = book.AvailableCopies;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        return null!;
    }
}
