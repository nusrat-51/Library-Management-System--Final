using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FinePaymentIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51578074-6fd7-40dd-8822-9c30648a44ce", "AQAAAAIAAYagAAAAEEpiWbT05aqzxsTndrpMHbCTOp5fBafmAMFmEtrIWw+H1l7D+vVh/tNDaSz6oVC2/Q==", "491192c3-9230-428e-882e-9d01c69ba7cc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2990286-6b4d-473c-bae9-b0ae6482197e", "AQAAAAIAAYagAAAAEL0Ne3FUnca54AlZ1nOb2TjjQaEyY/iBk4qjAchOl/wdVCIWmesiD1lTs8c6O0OVfA==", "04f20283-a94e-427b-be34-62d6944fb724" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d590bc91-841e-4e26-a42c-68680246298c", "AQAAAAIAAYagAAAAEAyflMB468tTsKwOPw7t7fDC64mkHVFBixWqzJKv30osbwhkQLn3S6afDVXzUuZLcQ==", "cedb1a18-46a5-4993-b35b-f8fa68f8fea6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
