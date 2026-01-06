using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class EmailNotificationLog
{
    [Key]
    public int Id { get; set; }

    public int BookApplicationId { get; set; }

    // "DueTomorrow" or "Overdue"
    [MaxLength(50)]
    public string NotificationType { get; set; } = "";

    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
}
