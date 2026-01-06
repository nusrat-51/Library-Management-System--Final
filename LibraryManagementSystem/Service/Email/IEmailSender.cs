using LibraryManagementSystem.Service.Email;

namespace LibraryManagementSystem.Service;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
}
