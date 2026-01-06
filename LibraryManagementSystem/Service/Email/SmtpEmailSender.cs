using LibraryManagementSystem.Service.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace LibraryManagementSystem.Service;

public class EmailSettings
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
}

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public SmtpEmailSender(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        using var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = message.Subject,
            IsBodyHtml = true
        };

        mail.To.Add(message.ToEmail);

        // Build HTML view so we can attach inline images via CID
        var htmlView = AlternateView.CreateAlternateViewFromString(message.HtmlBody, null, MediaTypeNames.Text.Html);

        // Inline images (logo)
        foreach (var kv in message.InlineImages)
        {
            var contentId = kv.Key; // e.g. "logo"
            var (content, contentType) = kv.Value;

            var stream = new MemoryStream(content);
            var resource = new LinkedResource(stream, contentType)
            {
                ContentId = contentId,
                TransferEncoding = TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(resource);
        }

        mail.AlternateViews.Add(htmlView);

        // Attachments (PDF)
        foreach (var a in message.Attachments)
        {
            var stream = new MemoryStream(a.Content);
            var attachment = new Attachment(stream, a.FileName, a.ContentType);
            mail.Attachments.Add(attachment);
        }

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = true
        };

        // SmtpClient doesn't support CancellationToken directly
        await client.SendMailAsync(mail);
    }
}
