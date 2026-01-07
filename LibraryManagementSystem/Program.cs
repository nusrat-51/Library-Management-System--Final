using CatMS;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helper;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Service;

using LibraryManagementSystem.Service.Email;
using LibraryManagementSystem.Service.Pdf;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;               // ✅ ADDED
using Microsoft.EntityFrameworkCore;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

builder.Services.AddSingleton<EmailTemplateBuilder>();
builder.Services.AddSingleton<EmailAssetLoader>();
builder.Services.AddSingleton<ReminderPdfGenerator>();

builder.Services.AddHostedService<ReturnReminderBackgroundService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookApplicationRepository, BookApplicationRepository>();
builder.Services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "LibraryAuth";
    options.Cookie.HttpOnly = true;

    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ISignInHelper, SignInHelper>();

builder.Services.AddHttpClient();


// =====================================================
// ✅ PREMIUM MEMBERSHIP + BARCODE UNLOCK (ADDED)
// =====================================================

// ✅ Needed for Session + Accessing HttpContext
builder.Services.AddHttpContextAccessor();

// ✅ Session support (for barcode unlock state)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Premium service + policy authorization
builder.Services.AddScoped<IPremiumAccessService, PremiumAccessService>();

// ✅ FIX: handler must NOT be Singleton (because it uses DbContext which is Scoped)
builder.Services.AddScoped<IAuthorizationHandler, PremiumHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PremiumOnly", policy =>
    {
        policy.Requirements.Add(new PremiumRequirement());
    });
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Session MUST be before Authentication/Authorization
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
