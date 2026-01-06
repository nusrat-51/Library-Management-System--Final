using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Service.Email;
using LibraryManagementSystem.Service.Pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace LibraryManagementSystem.Service;

public class ReturnReminderBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReturnReminderBackgroundService> _logger;

    
    private static readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
   

    public ReturnReminderBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ReturnReminderBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("✅ ReturnReminderBackgroundService STARTED at {time}", DateTime.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("🔁 Reminder job RUNNING at {time}", DateTime.Now);
                await RunOnce(stoppingToken);
                _logger.LogInformation("✅ Reminder job COMPLETED at {time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Reminder job FAILED");
            }

            try
            {
                _logger.LogInformation("⏳ Next run after: {interval}", _interval);
                await Task.Delay(_interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                
            }
        }

        _logger.LogInformation("🛑 ReturnReminderBackgroundService STOPPED at {time}", DateTime.Now);
    }

    private async Task RunOnce(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var templateBuilder = scope.ServiceProvider.GetRequiredService<EmailTemplateBuilder>();
        var assetLoader = scope.ServiceProvider.GetRequiredService<EmailAssetLoader>();
        var pdfGenerator = scope.ServiceProvider.GetRequiredService<ReminderPdfGenerator>();

       

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var issuedApps = await db.bookApplications
            .Where(x => x.Status == "Approved")
            .ToListAsync(ct);

        _logger.LogInformation("📌 Issued applications found: {count}", issuedApps.Count);

        foreach (var app in issuedApps)
        {
            if (string.IsNullOrWhiteSpace(app.StudentEmail))
            {
                _logger.LogWarning("⚠️ Skipping App #{id}: StudentEmail is empty", app.Id);
                continue;
            }

           
            var studentName = await ResolveStudentNameAsync(scope.ServiceProvider, app.StudentEmail /*, userManager*/, ct);

           
            if (app.ReturnDate.Date == tomorrow)
            {
                var alreadySent = await db.EmailNotificationLogs.AnyAsync(x =>
                    x.BookApplicationId == app.Id &&
                    x.NotificationType == "DueTomorrow" &&
                    x.SentAtUtc.Date == DateTime.UtcNow.Date, ct);

                if (!alreadySent)
                {
                    try
                    {
                        var (subject, html) = templateBuilder.BuildDueTomorrow(studentName, app);

                        var pdfBytes = pdfGenerator.CreateReminderPdf(
                            studentName: studentName,
                            app: app,
                            type: "DueTomorrow",
                            daysOverdue: null
                        );

                        var msg = new EmailMessage
                        {
                            ToEmail = app.StudentEmail,
                            Subject = subject,
                            HtmlBody = html,
                            InlineImages = new()
                            {
                                
                                ["logo"] = (assetLoader.LoadLogoPng(), "image/png")
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

                        await emailSender.SendAsync(msg, ct);

                        db.EmailNotificationLogs.Add(new EmailNotificationLog
                        {
                            BookApplicationId = app.Id,
                            NotificationType = "DueTomorrow",
                            SentAtUtc = DateTime.UtcNow
                        });
                        await db.SaveChangesAsync(ct);

                        _logger.LogInformation("📧 DueTomorrow email SENT to {email} for App #{id}", app.StudentEmail, app.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ DueTomorrow email FAILED for App #{id} to {email}", app.Id, app.StudentEmail);
                    }
                }
                else
                {
                    _logger.LogInformation("✅ DueTomorrow already sent today for App #{id}", app.Id);
                }
            }

            
            if (app.ReturnDate.Date < today)
            {
                var alreadySent = await db.EmailNotificationLogs.AnyAsync(x =>
                    x.BookApplicationId == app.Id &&
                    x.NotificationType == "Overdue" &&
                    x.SentAtUtc.Date == DateTime.UtcNow.Date, ct);

                if (!alreadySent)
                {
                    var daysOverdue = (today - app.ReturnDate.Date).Days;

                    try
                    {
                        var (subject, html) = templateBuilder.BuildOverdue(studentName, app, daysOverdue);

                        var pdfBytes = pdfGenerator.CreateReminderPdf(
                            studentName: studentName,
                            app: app,
                            type: "Overdue",
                            daysOverdue: daysOverdue
                        );

                        var msg = new EmailMessage
                        {
                            ToEmail = app.StudentEmail,
                            Subject = subject,
                            HtmlBody = html,
                            InlineImages = new()
                            {
                                ["logo"] = (assetLoader.LoadLogoPng(), "image/png")
                            },
                            Attachments = new()
                            {
                                new EmailAttachment
                                {
                                    FileName = $"OverdueNotice_App{app.Id}.pdf",
                                    ContentType = "application/pdf",
                                    Content = pdfBytes
                                }
                            }
                        };

                        await emailSender.SendAsync(msg, ct);

                        db.EmailNotificationLogs.Add(new EmailNotificationLog
                        {
                            BookApplicationId = app.Id,
                            NotificationType = "Overdue",
                            SentAtUtc = DateTime.UtcNow
                        });
                        await db.SaveChangesAsync(ct);

                        _logger.LogInformation("📧 Overdue email SENT to {email} for App #{id}", app.StudentEmail, app.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Overdue email FAILED for App #{id} to {email}", app.Id, app.StudentEmail);
                    }
                }
                else
                {
                    _logger.LogInformation("✅ Overdue already sent today for App #{id}", app.Id);
                }
            }
        }
    }

    
    private static async Task<string> ResolveStudentNameAsync(IServiceProvider sp, string email /*, UserManager<IdentityModel> userManager*/, CancellationToken ct)
    {
      
        var prefix = email.Split('@')[0];
        if (string.IsNullOrWhiteSpace(prefix)) return "Student";

        prefix = prefix.Replace(".", " ").Replace("_", " ").Trim();
        return char.ToUpper(prefix[0]) + prefix.Substring(1);
    }
}
