using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixPremiumMembershipNullables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "Expiry",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86f60621-7160-4ece-ac85-cb6cb4d7ce90", "AQAAAAIAAYagAAAAEMgBQQ4IE6rfpHBODu8Oyn/f7gx7kbGJRKVfMVgqmwLmThYvuVKvP7kYKrERp+UEzw==", "94f0ba9f-32d8-41f1-b651-b9823d5cd967" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "630826c1-9bcd-4a26-8857-c27abfe7286d", "AQAAAAIAAYagAAAAEHGFRCWA9awzyuYfFKH4ltFvRcDq6sacTqwOGiIyBsVKF7uJ9w0M7D9ueSnkKMjPWg==", "1513efee-e86d-45df-a975-7136d6eb82d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7080f0d0-0133-4507-aa4b-ee5cd47c9c11", "AQAAAAIAAYagAAAAEK6C2qxK7qVQ4/ZSjQRu7yEHqMQdcm1+wS87gBqqzIhSwvOKO9sKkE+Aw7aECsA1bw==", "a2903cfc-2bf9-4454-b828-2b31d6826a38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expiry",
                table: "PremiumMemberships");

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
    }
}
