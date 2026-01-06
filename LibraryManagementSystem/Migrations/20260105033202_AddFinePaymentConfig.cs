using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFinePaymentConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ede5e4f0-c191-48b4-b22a-249953051e9e", "AQAAAAIAAYagAAAAEMDfSTB2e72l/QYBaEyZpTIGgPNecVrnO3mUKORsIMzK4od+7qNs9Ku2UsSZXCZb8A==", "347ac391-8466-4de2-ac53-333c142af78e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9fd37ea-2a5a-4633-8d74-f72153ca1d90", "AQAAAAIAAYagAAAAEI3AvKnByGwAbwEUJDwUBLL75rw+GTDN8KQG82MKh9fRvp/uomFK7d7wldaPk5hJ/w==", "f7bf321f-3995-48d7-8918-2e3fd6a9b34a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "137ff58a-410e-4bd2-9dd3-f4c3f2ff75bf", "AQAAAAIAAYagAAAAEFX4wVegShUfEQAlCosEabjI8+3xVA/hBdqL1Q56QqSQNINIKVi82oLEuRttcU8ZKQ==", "181977ae-de34-493d-90ee-7c96f2e45f3e" });

            migrationBuilder.CreateIndex(
                name: "IX_FinePayments_BookApplicationId",
                table: "FinePayments",
                column: "BookApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinePayments_TranId",
                table: "FinePayments",
                column: "TranId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments",
                column: "BookApplicationId",
                principalTable: "bookApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments");

            migrationBuilder.DropIndex(
                name: "IX_FinePayments_BookApplicationId",
                table: "FinePayments");

            migrationBuilder.DropIndex(
                name: "IX_FinePayments_TranId",
                table: "FinePayments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "501bf5ef-ec16-4540-a43a-89bbadd7a90b", "AQAAAAIAAYagAAAAEKQ1XV//8MOWbPf0OoRSznB/yNabUTRNKoZFntCKqRQftjVgiOqAqWCl3+4l/1F7TQ==", "8f50b1c0-0e83-475f-9b49-f519d8fcb987" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7c81d3e-9aa0-43dd-903d-e4dc9ab057d0", "AQAAAAIAAYagAAAAELjhdEYPOg7NMEB0oFdcf7Ai2+BXHXQ67fGvalwzShbBTQK8yVKrK9bYf/pr7B6vCA==", "8dcb98b7-41b9-4f31-8a2a-f6df0f928de5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32294296-471d-4892-8088-54eb6d7c2160", "AQAAAAIAAYagAAAAEBLc8LIAFhDYdrwudcnZxYZpLXGVO6wM+pO8SZJzmrXVkxy5QhT9M22d29ttJ5i7qg==", "bab89c28-3d73-4a00-a1de-7dbc9f37d6c6" });
        }
    }
}
