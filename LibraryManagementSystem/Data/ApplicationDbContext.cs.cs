using LibraryManagementSystem.Auth_IdentityModel;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

namespace LibraryManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<
    IdentityModel.User,
    IdentityModel.Role,
    long,
    IdentityModel.UserClaim,
    IdentityModel.UserRole,
    IdentityModel.UserLogin,
    IdentityModel.RoleClaim,
    IdentityModel.UserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookApplication> bookApplications { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<EmailNotificationLog> EmailNotificationLogs { get; set; }

    // Fine payment table
    public DbSet<FinePayment> FinePayments { get; set; }
    public DbSet<LibraryManagementSystem.Models.PremiumMembership> PremiumMemberships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Book>()
            .HasOne(b => b.bookCategory)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<BookApplication>()
            .HasOne(ba => ba.Book)
            .WithMany()
            .HasForeignKey(ba => ba.BookId)
            
            .OnDelete(DeleteBehavior.Cascade);

       
        modelBuilder.Entity<FinePayment>()
            .HasOne(fp => fp.BookApplication)
            .WithMany()
            .HasForeignKey(fp => fp.BookApplicationId)
           
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<FinePayment>()
            .HasIndex(fp => fp.TranId)
            .IsUnique();

        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        optionsBuilder.LogTo(Console.WriteLine);

        optionsBuilder.UseLoggerFactory(
            new LoggerFactory(new[]
            {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            })
        );
    }
}
