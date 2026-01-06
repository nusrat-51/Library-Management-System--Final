using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinePaymentStudentIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "FinePayments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "449d79fe-de05-4a0b-8e3f-93ad895fc3a5", "AQAAAAIAAYagAAAAEDwVE3fAqoISMnrkp3ZJmWDGx4Zo6mPsvh5Id6r03fCYUP51QX4iX5/Y1UJTy+IBBw==", "79d19422-174e-4f44-a78c-be09fbdfd5aa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4f55c38-7958-458e-9f57-97ba9ab16021", "AQAAAAIAAYagAAAAEHca/hgvjbXnMFAONUlIZd9uoOD4f10vcLIauOzGDVA4Oh4+hxRn6eywA9TbKrTakw==", "c0e8e24e-c0bb-47dd-8ad7-e55eac9acb75" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3ac22aa-b721-475d-9057-e21b2176db18", "AQAAAAIAAYagAAAAEOFAx1QSDJ3xXGvOo5Zu2OAi1SwvIbdTBEk/VjXtaQY6lHRkUt8N3GsJ3wwxy+iG2A==", "8de90b69-e72f-4751-a1a5-3b6b933174e2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "FinePayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

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

    }
}
