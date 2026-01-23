using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPremiumMembershipPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "PremiumMemberships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "PremiumMemberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gateway",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TranId",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValId",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "Gateway",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "TranId",
                table: "PremiumMemberships");

            migrationBuilder.DropColumn(
                name: "ValId",
                table: "PremiumMemberships");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d389d293-3612-4af7-8e2e-ba106687c697", "AQAAAAIAAYagAAAAEAnS3I3vwGWqO/Pz3LNiyvLaAm2YAHTHunCxuIsZtyhUtgXcOuT5rWd0t/Gkx4cigQ==", "ad67bf08-0273-4c40-911d-ded1d22280f7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12249e99-6c06-480b-8e1c-e2d9adc77ec4", "AQAAAAIAAYagAAAAEA1rhM8s3tWrWNwHf04r9vn27wLZx/r6J1iaKgLbGDnWTmL2Ywaxe4nZw5Brbhduig==", "03abecec-4163-4ac5-a946-4c6ec3dccbfe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "efc393bd-648a-4e13-9749-b2ae0ee46196", "AQAAAAIAAYagAAAAEDYDIT8p2vl+fGpDEzwq3f6IvS02zMDt6hwDgaRNSyGaJbbD1Lp53AhMVd6G0GLDrA==", "f15599fb-1908-4903-b0bb-96e9ae2c7b5a" });
        }
    }
}
