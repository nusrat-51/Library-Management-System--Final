using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository;

public class BookApplicationRepository : IBookApplicationRepository
{
    private readonly ApplicationDbContext _context;
    public BookApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BookApplication> AddBookApplicationAsync(BookApplication bookApplication, CancellationToken cancellationToken)
    {
        await _context.bookApplications.AddAsync(bookApplication, cancellationToken);
        await  _context.SaveChangesAsync(cancellationToken);
        return bookApplication;
    }

    public async Task<BookApplication> DeleteBookApplicationAsync(int id, CancellationToken cancellationToken)
    {
       var data = await _context.bookApplications.FindAsync(id);
         if (data != null)
         {
                _context.bookApplications.Remove(data);
                await _context.SaveChangesAsync(cancellationToken);
                return data;
        }
         return null!;
    }

  
    public async Task<IEnumerable<BookApplication>> GetAllBookApplicationAsync(CancellationToken cancellationToken)
    {
       var data = await _context.bookApplications.Include(x=>x.Book).ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        return null;
    }

    public async Task<BookApplication> GetBookApplicationByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.bookApplications.FindAsync(id);
        if (data != null)
        {
            return data;
        }
        return null!;
    }


    public async Task<BookApplication> UpdateBookApplicationAsync(BookApplication bookApplication, CancellationToken cancellationToken)
    {
       var data = await _context.bookApplications.FindAsync(bookApplication.Id);
         if (data != null) 
         { 
              data.StudentEmail = bookApplication.StudentEmail;
              data.StudentId = bookApplication.StudentId;
              data.StudentIdCardNo = bookApplication.StudentIdCardNo;
              data.Status = bookApplication.Status;
              data.FineAmount = bookApplication.FineAmount;
              data.IssueDate = bookApplication.IssueDate;
              data.ReturnDate = bookApplication.ReturnDate;
              //data.BookId = bookApplication.BookId;
              await _context.SaveChangesAsync(cancellationToken);
              return data;
        }
            return null!;
    }

}
