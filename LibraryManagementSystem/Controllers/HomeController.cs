using System.Diagnostics;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

using LibraryManagementSystem.Service;
using LibraryManagementSystem.Service.Email;
using LibraryManagementSystem.Service.Pdf;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IEmailSender _emailSender;
        private readonly EmailTemplateBuilder _template;
        private readonly EmailAssetLoader _assets;
        private readonly ReminderPdfGenerator _pdf;

        public HomeController(
            ILogger<HomeController> logger,
            IEmailSender emailSender,
            EmailTemplateBuilder template,
            EmailAssetLoader assets,
            ReminderPdfGenerator pdf)
        {
            _logger = logger;

            _emailSender = emailSender;
            _template = template;
            _assets = assets;
            _pdf = pdf;
        }

        // ? Landing page (Public)
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // ? Optional: Privacy page (Public)
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        // ? NEW: Contact page (Public) — doesn't affect any existing features
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // ? Keep your test mail endpoint (Public only for testing)
        // Tip: In real production, you would restrict this to Admin, but I'm NOT changing your logic.
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TestUniversityMail(string to, string name = "Student", int appId = 2002)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(to))
                    return BadRequest("Please provide 'to' email. Example: /Home/TestUniversityMail?to=someone@gmail.com");

                if (string.IsNullOrWhiteSpace(name))
                    name = "Student";

                // Fake application data for email preview/testing
                var app = new BookApplication
                {
                    Id = appId,
                    StudentEmail = to,
                    ReturnDate = DateTime.Today.AddDays(1), // Due tomorrow
                    Status = "Approved"
                };

                // Build email
                var (subject, html) = _template.BuildDueTomorrow(name, app);

                // Generate PDF
                var pdfBytes = _pdf.CreateReminderPdf(name, app, "DueTomorrow");

                // Compose message
                var msg = new EmailMessage
                {
                    ToEmail = to,
                    Subject = subject,
                    HtmlBody = html,
                    InlineImages = new()
                    {
                        ["logo"] = (_assets.LoadLogoPng(), "image/png")
                    },
                    Attachments = new()
                    {
                        new EmailAttachment
                        {
                            FileName = $"DueTomorrowNotice_App{app.Id}.pdf",
                            ContentType = "application/pdf",
                            Content = pdfBytes
                        }
                    }
                };

                await _emailSender.SendAsync(msg);

                return Content($"? Test email sent successfully to {to}. Check Inbox + Spam.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? TestUniversityMail failed");
                return StatusCode(500, "? Failed to send test email. Check logs/output for error details.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
