using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFinePaidToBookApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinePaid",
                table: "bookApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0ebefa31-1335-41ca-9107-43e8b46d3297", "AQAAAAIAAYagAAAAEAeIaXIKBqYxeiLJH3Y+xCfDGlecP1fnCe58E/AZxmNSYR2A4oD4ijs50+lFxChchg==", "36fed098-ffb2-4d5f-9d5f-6214a56c3d7d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07034aca-55fb-4230-b823-bc188b1cc727", "AQAAAAIAAYagAAAAEMzjX1C2yG4Cmru/5lJKtk/ih86mjupWqmxyvaPVwBj2nBszuyYR8SizeyfGnaoHLg==", "bdffd66a-04ce-4bda-8801-2b4d4dc5a978" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05dee456-0363-4569-bce8-f4e12066ef65", "AQAAAAIAAYagAAAAEDru4Ygcr6bTlvaznOpjomJ5Plx5p4ufmCKrBWNWjjKp1Q4JC73/1Q0MMTh63A+8WQ==", "5acd20aa-f7cf-4c31-827c-722e5a5b67fb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinePaid",
                table: "bookApplications");

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
    }
}
