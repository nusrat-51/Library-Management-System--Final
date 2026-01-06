using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailNotificationLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookApplicationId = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotificationLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c40da3b1-db0a-4925-a89f-e6921fd5c185", "AQAAAAIAAYagAAAAENfq4/6SFdTF175yW/AFmc670vq2PN/9unUFt3RksX2MNhPuhooKvDVoLVI8F+2WTA==", "c1be4a45-1a74-404b-ad4b-639c40bcd019" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0bca1839-7e83-41d7-867a-1986f4f7f6e3", "AQAAAAIAAYagAAAAEOYMDjxA2/88Xs0Nx3tSmBsp+RF4LSW6krLb/sxCIFEmo+AK43L3Awki4H3O8uPdeA==", "6081df66-4c1b-4430-8b05-ecce9a06f4bd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07a079bd-a981-4145-85f0-a5864dc3ff5e", "AQAAAAIAAYagAAAAECqzCPXW5v+93TLY2OvCEcq6okZLcexQ5pass7u+chHnuJxYwbwFBa1hJXKcOX0Lig==", "5ac701ab-5c38-4d18-a58d-64a6b28ab576" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotificationLogs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "760b0e1b-6c98-4969-96ea-14cba4863080", "AQAAAAIAAYagAAAAEJ688fTqZB78ptYpR6KxiQsWLAFT58N3ppS9btaNGLYvWUZ593vM8mQrFGbshy028Q==", "715f997c-8b34-4f85-85c0-8705051349da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6308fd22-d676-4023-8f93-383be216ec4f", "AQAAAAIAAYagAAAAELhaQox2DqkSKnPR/GHBVSxe09gYW/ZEh9NfdcQ8tyeYuA8VF/fIJzE+CIwyUjd7og==", "c8e8bac4-942d-43cd-a95a-eb1170c0702e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f736058-c7a0-4d30-822f-c56f2de48bce", "AQAAAAIAAYagAAAAEOF7ZupbW5vORloa1WEOL9Y6CNZ3jy4IVHf514EevLnIp3A5dYnfpkmnuywO2rtGuw==", "c431c99c-24fe-4cfb-95c9-44ac122d4a6e" });
        }
    }
}
