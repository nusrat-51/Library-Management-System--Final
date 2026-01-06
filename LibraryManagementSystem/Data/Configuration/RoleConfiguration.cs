using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

namespace LibraryManagementSystem.Data.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(new Role
        {
            Id = 1,
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Description = "Default role assigned to all employees."

        }, new Role
        {
            Id = 2,
            Name = "Student",
            NormalizedName = "STUDENT",
            Description = "Default role assigned to all employees."
        }, new Role
        {
            Id = 3,
            Name = "Mangement",
            NormalizedName = "MANGEMENT",
            Description = "Default role assigned to all customers."
        }

        );
    }
}

