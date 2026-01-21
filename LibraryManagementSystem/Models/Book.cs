using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using LibraryManagementSystem.BaseEntities;

namespace LibraryManagementSystem.Models
{
    public class Book : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public int CategoryId { get; set; }

        [ValidateNever]
        public BookCategory? bookCategory { get; set; }

        [ValidateNever]
        public ICollection<BookApplication> BookApplications { get; set; } = new List<BookApplication>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPremium { get; set; } = false;
    }
}
