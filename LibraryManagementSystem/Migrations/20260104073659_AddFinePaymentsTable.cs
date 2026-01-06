using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFinePaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookApplicationId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudentEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TranId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankTranId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinePayments", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab3f417a-c1f2-4b33-904f-311a7c84d330", "AQAAAAIAAYagAAAAEPnhq56oKUUJht2x0xQf/aZ3PcAr8si9mXDDRsorEKyqOrS+O91o45DxDwsF0d5kDg==", "d102300d-bac3-4035-adc9-0d4b23137e31" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f94a529-2fe9-47e4-b32c-03ad3ed10c40", "AQAAAAIAAYagAAAAEJbvKnnKFTUNbWUfDmkO8g0Z7ngcbik+y8bZArLc9ehZYJvb3dvnJq3cJWN92GtQ3Q==", "cff18c81-56c9-4382-be89-9d2f26a3b0b6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a268ed9-dbef-4c4d-a982-65d6d4be8163", "AQAAAAIAAYagAAAAEF8F31WVjH0uLkQ/DeNsjgqgW3+WV51Fjup/HIiwIKfurYTtWhEAKFSgiMBHaKee+A==", "5860afb6-48ff-4cfc-be51-c8d06a569803" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinePayments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "719e4788-c701-42d2-8294-859460024c0c", "AQAAAAIAAYagAAAAEOKkf7CWkUd46klL7TmXwp3XBpWAN8fwrmf4yFSKzwiygL4MISWuBL3BTiwVZ25vmQ==", "55ae62c9-e3f2-49ca-b547-a0914766e167" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e23c48d9-01f1-4a25-8b3f-f30907a498a6", "AQAAAAIAAYagAAAAEGiVa68jBifZxfE8rjrQQhj1MnOha90odvhbALfMJbQksuwbwgGw8pe84DkCUwLejg==", "d6508cd0-1f05-4213-8050-dbe838ad349b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c5b7473c-366b-4bf5-91de-ec49e57d3352", "AQAAAAIAAYagAAAAEEN6X6+EOeBLp6MxhI/Sl5IdSD0Gq+NAdN/faOwxBQCURIX4bv1S+JuZGffFqAagvA==", "e3cb4440-aa83-48c1-be80-3cf1a2d3670e" });
        }
    }
}
