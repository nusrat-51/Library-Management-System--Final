using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBookCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f39484db-4175-470b-86c5-5a501debeb83", "AQAAAAIAAYagAAAAEKOakHrmfDBSpzSzuudQrp6G6Ev4JZKmNKzGEc2zBiXpwoy9cPiPhDQCX+an+W/iAg==", "340069b4-d1e2-4680-bfc4-d82b9bbddbe7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a3185490-3384-4713-9e4e-a20939151169", "AQAAAAIAAYagAAAAENwgUtBWXT/a6WUz2IDSXtIH2e98NeEPHFP7a2cy/LPIRQJaRQXFIcpB5WL5u0eNmQ==", "9611c271-cd69-4a46-91cb-27b26f9c91ea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "237fc13c-955f-4be8-894e-b64ce6574c3b", "AQAAAAIAAYagAAAAEECbG/cVy1Kzo5l/j4V7SnQuWQJXyXkOiYy1vZhjaSFrskx4WcieURemKW7lmwBlzw==", "64e8debd-4346-441f-a57a-7fd676dc5f4c" });
        }
    }
}
