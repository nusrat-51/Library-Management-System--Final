using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPremiumToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "Books");

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
        }
    }
}
