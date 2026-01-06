using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Service.Email;

public class EmailTemplateBuilder
{
    private static string Layout(string title, string bodyHtml)
    {
        // CID logo: <img src="cid:logo" />
        return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8' />
</head>
<body style='margin:0;padding:0;background:#f5f6f8;font-family:Arial,Helvetica,sans-serif;'>
  <div style='max-width:640px;margin:24px auto;background:#ffffff;border:1px solid #e5e7eb;border-radius:10px;overflow:hidden;'>
    
    <div style='padding:16px 20px;background:#0f172a;color:#ffffff;'>
      <div style='display:flex;align-items:center;gap:12px;'>
        <img src='cid:logo' alt='Library' style='height:40px;width:40px;border-radius:6px;object-fit:contain;background:#ffffff;padding:4px;' />
        <div>
          <div style='font-size:16px;font-weight:700;line-height:1.2;'>Library Management System</div>
          <div style='font-size:12px;opacity:0.9;'>Automated Notification</div>
        </div>
      </div>
    </div>

    <div style='padding:20px;'>
      <h2 style='margin:0 0 12px 0;font-size:18px;color:#111827;'>{title}</h2>
      <div style='font-size:14px;color:#111827;line-height:1.6;'>{bodyHtml}</div>
    </div>

    <div style='padding:14px 20px;background:#f9fafb;border-top:1px solid #e5e7eb;font-size:12px;color:#6b7280;line-height:1.5;'>
      <div><strong>Note:</strong> This is an automated email. Please do not reply.</div>
      <div style='margin-top:6px;'>Regards,<br/>Library Administration</div>
    </div>

  </div>
</body>
</html>";
    }

    public (string Subject, string Html) BuildDueTomorrow(string studentName, BookApplication app)
    {
        var subject = "Reminder: Book Return Due Tomorrow";
        var body = $@"
<p>Dear <strong>{studentName}</strong>,</p>

<p>
This is a friendly reminder that the book issued under
<strong>Application ID #{app.Id}</strong> is due for return on
<strong>{app.ReturnDate:dd MMM yyyy}</strong>.
</p>

<p>
Please return the book by the due date to avoid any overdue fines.
</p>

<p>
If you have already returned the book, kindly disregard this message.
</p>

<p>Thank you for your cooperation.</p>";

        return (subject, Layout("Book Return Reminder", body));
    }

    public (string Subject, string Html) BuildOverdue(string studentName, BookApplication app, int daysOverdue)
    {
        var subject = "Overdue Book Notice – Immediate Attention Required";
        var body = $@"
<p>Dear <strong>{studentName}</strong>,</p>

<p>
This is to inform you that the book issued under
<strong>Application ID #{app.Id}</strong> has exceeded its return date and is currently
<strong>{daysOverdue} day(s) overdue</strong>.
</p>

<p>
We kindly request you to return the book to the library at your earliest convenience
to avoid further fines or penalties.
</p>

<p>
If you have already returned the book, please disregard this message.
</p>

<p>Thank you for your cooperation.</p>";

        return (subject, Layout("Overdue Book Notice", body));
    }
}
