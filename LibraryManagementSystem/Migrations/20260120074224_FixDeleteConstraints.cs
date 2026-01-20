using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookApplications_Books_BookId",
                table: "bookApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "PremiumMemberships");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "PremiumMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0ef4e727-06c2-4b78-91fa-be8ad98e1285", "AQAAAAIAAYagAAAAEKXXZ9ROgoOAhwMmF0vpDSPe0SYVJn+nOZD+YpDlSrjU9GDp9CLb8dhBAbflD+CuLA==", "13c0146e-658a-453a-b0de-172e90660138" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4e9c2ca-07d7-4313-8115-9630842f1275", "AQAAAAIAAYagAAAAED1H6MKK3QYIznKua+S2PljvePmx0AQ0cc2LH7r/sQ0U8OJZbEP1uav1OJlioYQ72A==", "b2b1dce1-17db-4a36-8e44-7dd864804bed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3e859bac-2933-4400-ad76-91fd378abf43", "AQAAAAIAAYagAAAAELSk9q1prIRToCZqVvffDVRnnruTqtOZlHStFN9hhsilDKTiqTgIULmV9yKFYshnhw==", "468bcafa-bf70-43bd-934b-bbf4129a6841" });

            migrationBuilder.AddForeignKey(
                name: "FK_bookApplications_Books_BookId",
                table: "bookApplications",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments",
                column: "BookApplicationId",
                principalTable: "bookApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookApplications_Books_BookId",
                table: "bookApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "PremiumMemberships");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "PremiumMemberships",
                type: "datetime2",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_bookApplications_Books_BookId",
                table: "bookApplications",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinePayments_bookApplications_BookApplicationId",
                table: "FinePayments",
                column: "BookApplicationId",
                principalTable: "bookApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
