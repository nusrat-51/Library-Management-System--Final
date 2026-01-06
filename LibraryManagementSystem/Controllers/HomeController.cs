using System.Diagnostics;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;


using LibraryManagementSystem.Service;
using LibraryManagementSystem.Service.Email;
using LibraryManagementSystem.Service.Pdf;

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> TestUniversityMail(string to, string name = "Student", int appId = 2002)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(to))
                    return BadRequest("Please provide 'to' email. Example: /Home/TestUniversityMail?to=someone@gmail.com");

               
                var app = new BookApplication
                {
                    Id = appId,
                    StudentEmail = to,
                    ReturnDate = DateTime.Today.AddDays(1), // Due tomorrow
                    Status = "Approved"
                };

               
                var (subject, html) = _template.BuildDueTomorrow(name, app);

                
                var pdfBytes = _pdf.CreateReminderPdf(name, app, "DueTomorrow");

             
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

                return Content("? Test email sent successfully. Check Inbox + Spam + Sender 'Sent' folder.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? TestUniversityMail failed");
                return StatusCode(500, "? Failed to send test email. Check Output window for error details.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
