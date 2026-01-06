namespace LibraryManagementSystem.Service.Email;

public class EmailMessage
{
    public string ToEmail { get; set; } = "";
    public string Subject { get; set; } = "";
    public string HtmlBody { get; set; } = "";

    // Optional: inline images like logo
    public Dictionary<string, (byte[] Content, string ContentType)> InlineImages { get; set; } = new();

    // Optional: attachments like PDFs
    public List<EmailAttachment> Attachments { get; set; } = new();
}
