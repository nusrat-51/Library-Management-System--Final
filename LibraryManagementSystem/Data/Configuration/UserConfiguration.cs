using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

namespace LibraryManagementSystem.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var hasher = new PasswordHasher<User>();
        builder.HasData(new User
        {
            Id = 1,
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            UserName = "admin@localhost.com",
            NormalizedUserName = "ADMIN@LOCALHOST.COM",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        }, new User
        {
            Id = 2,
            Email = "employee@localhost.com",
            NormalizedEmail = "EMPLOYEE@LOCALHOST.COM",
            UserName = "employee@localhost.com",
            NormalizedUserName = "EMPLOYEE@LOCALHOST.COM",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        },
        new User
        {
            Id = 3,
            Email = "Manager@localhost.com",
            NormalizedEmail = "Manager@LOCALHOST.COM",
            UserName = "Manager@localhost.com",
            NormalizedUserName = "Manager@LOCALHOST.COM",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        });
    }
}
