using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class MakeValIdNullableInPremiumMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValId",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54c4aa01-13f1-4dae-998d-399fef1b2470", "AQAAAAIAAYagAAAAENn1LDMRfuZ2hqOohce8zkKSxR9YzdeR96V3X+D+PC2sXa3gWYACtlIFrqH0vEV8qA==", "b8ab7f1b-15e7-4b95-b373-db8a8a1302a9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "293e26e0-a6e6-4aba-a3f2-40caeb272a11", "AQAAAAIAAYagAAAAEPKFlU2MA/kku97HSoqqpRl3e7c/86BexK2iICVCUrTzpQ2vT8SZIXFoR8S8LxTc6A==", "b2aef120-9156-4ef1-b958-90bd90ff38bd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "854d26c4-d2ed-453f-81c0-c906d716d9cd", "AQAAAAIAAYagAAAAEMMFU5o7NHSvJgbQEf2OP0WcwR8DS3ay6aYyahTDlEeHvS7WQblSdJmuk9LE9jNPXA==", "5c0d66b6-e22d-494d-b4b8-e4e224b8a9b8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValId",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed949548-d13d-4fe7-9f51-2e934ef8f2d4", "AQAAAAIAAYagAAAAEEVtTH5qdM+wi7Is+P0RV13B1Vcx1VWo17rKOxrvqsoSjTtAUtl8wsTku6VRqUS4dg==", "b116424d-c872-4c00-8f25-0ffeea56d9af" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54953dbc-e2bf-469e-a897-3959cb93a7f3", "AQAAAAIAAYagAAAAECmBDPbcNgVeNGSIE6GTD7NefC7OYvFoCxST/8BcjWTqemMpO9FHKzto3eQilkf5Tw==", "c5de50d2-7de8-4971-9e5c-ec9b72a2dd36" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26263a47-5cc0-4ba7-aa9e-db406cc4355d", "AQAAAAIAAYagAAAAEBy5d5vllvK4Jbs4cJ+PlRQn4fCiKHxvsIpeWJ5kRFvxwY2wX0UuYEniik+6HJlXyA==", "5e9d5f87-7c5a-445a-9172-5666560e5c1a" });
        }
    }
}
