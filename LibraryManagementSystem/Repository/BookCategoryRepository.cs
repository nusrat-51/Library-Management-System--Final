using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository;

public class BookCategoryRepository : IBookCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public BookCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BookCategory> AddBookCategoryAsync(BookCategory bookCategory, CancellationToken cancellationToken)
    {
      await _context.BookCategories.AddAsync(bookCategory, cancellationToken);
      await  _context.SaveChangesAsync(cancellationToken);
        return bookCategory;
    }

    public async Task<BookCategory> DeleteBookCategoryAsync(int id, CancellationToken cancellationToken)
    {
       var data= await _context.BookCategories.FindAsync(id);
         if (data != null)
         {
                _context.BookCategories.Remove(data);
                await _context.SaveChangesAsync(cancellationToken);
                return data;
        }
         return null!;
    }


    public IEnumerable<SelectListItem> Dropdown()
    {
        var data = _context.BookCategories.Select(x => new SelectListItem
        {
            Text = x.CategoryName,
            Value = x.Id.ToString()
        }).ToList();
        return data;
    }

    public async Task<IEnumerable<BookCategory>> GetAllBookCategoryAsync(CancellationToken cancellationToken)
    {
       var data = await _context.BookCategories.ToListAsync(cancellationToken);
        if (data != null)
        {
               return data ?? Enumerable.Empty<BookCategory>();
        }
        return null!;
    }

    public async Task<BookCategory> GetBookCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.BookCategories.FindAsync(id);
        if (data != null)
        {
            return data;
        }
        return null!;
    }

    public async Task<BookCategory> UpdateBookCategoryAsync(BookCategory bookCategory, CancellationToken cancellationToken)
    {
        var data = await _context.BookCategories.FindAsync(bookCategory.Id);
        if (data != null) 
        {
            data.Description = bookCategory.Description;
            data.CategoryName = bookCategory.CategoryName;
            data.IsActive = bookCategory.IsActive;
            data.CreatedAt = bookCategory.CreatedAt;
            await _context.SaveChangesAsync(cancellationToken);
            return data;

        }
        return null!;
    }
}
