namespace LibraryManagementSystem.Models;

public class Book: BaseEntities.BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public int CategoryId { get; set; }
    public BookCategory?  bookCategory { get; set; }
    public ICollection<BookApplication> BookApplications { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
