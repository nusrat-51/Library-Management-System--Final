using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPremiumMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PremiumMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarcodeHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumMemberships", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c83620f9-8b65-4f04-b861-25fd766cada1", "AQAAAAIAAYagAAAAEFuaDbT+AC0z55GpPU00neuBuZHe3D8Oe6elBTfKFbLECnAeS9Ok23lLnta4w9H1mw==", "9e788202-ee69-4134-95ad-e98f5786fc9d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "976452c6-ae09-463e-b061-9fd3dcf02aaa", "AQAAAAIAAYagAAAAEHXsB64OvqINapicimpOZLubjctZBj6PErC01DsD/PghPWUBHxUwg4UNTCeFXHl7hw==", "ff7244c2-764c-4c6c-bb18-7fac4c1395b2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15c8a3c5-03a5-45b9-81cf-0735bb7dbff3", "AQAAAAIAAYagAAAAELeB4/76SjeK15hPlpbtyodnkvH+B3zOPNIXXeooOrcKGNkjCF+etaQIA4ayFGp6mw==", "b2cdd536-32a3-4458-b172-3a7e9b9cccd2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PremiumMemberships");

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
    }
}
