using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class BookCategory: BaseEntities.BaseEntity<int>
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Book>? Books { get; set; }
}
