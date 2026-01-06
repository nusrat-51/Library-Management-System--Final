using LibraryManagementSystem.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LibraryManagementSystem.Service.Pdf;

public class ReminderPdfGenerator
{
    public byte[] CreateReminderPdf(string studentName, BookApplication app, string type, int? daysOverdue = null)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Content().Column(col =>
                {
                    col.Item().Text("Library Management System")
                        .FontSize(18).Bold();

                    col.Item().Text("Book Return Notice")
                        .FontSize(14).SemiBold()
                        .FontColor(Colors.Grey.Darken2);

                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text($"Date: {DateTime.Now:dd MMM yyyy}");
                    col.Item().Text($"Student: {studentName}");
                    col.Item().Text($"Application ID: #{app.Id}");
                    col.Item().Text($"Return Date: {app.ReturnDate:dd MMM yyyy}");

                    if (type == "Overdue" && daysOverdue.HasValue)
                        col.Item().Text($"Status: Overdue by {daysOverdue.Value} day(s)").Bold();

                    if (type == "DueTomorrow")
                        col.Item().Text("Status: Due tomorrow").Bold();

                    col.Item().PaddingTop(16).Text("Please return the issued book by the due date to avoid fines.")
                        .FontSize(12);

                    col.Item().PaddingTop(30).Text("Regards,")
                        .FontSize(12);
                    col.Item().Text("Library Administration")
                        .FontSize(12).Bold();
                });
            });
        });

        return doc.GeneratePdf();
    }
}
