using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFinePaymentBookApplicationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "760b0e1b-6c98-4969-96ea-14cba4863080", "AQAAAAIAAYagAAAAEJ688fTqZB78ptYpR6KxiQsWLAFT58N3ppS9btaNGLYvWUZ593vM8mQrFGbshy028Q==", "715f997c-8b34-4f85-85c0-8705051349da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6308fd22-d676-4023-8f93-383be216ec4f", "AQAAAAIAAYagAAAAELhaQox2DqkSKnPR/GHBVSxe09gYW/ZEh9NfdcQ8tyeYuA8VF/fIJzE+CIwyUjd7og==", "c8e8bac4-942d-43cd-a95a-eb1170c0702e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f736058-c7a0-4d30-822f-c56f2de48bce", "AQAAAAIAAYagAAAAEOF7ZupbW5vORloa1WEOL9Y6CNZ3jy4IVHf514EevLnIp3A5dYnfpkmnuywO2rtGuw==", "c431c99c-24fe-4cfb-95c9-44ac122d4a6e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c12201e0-dce8-4955-8df5-0e7da5eeb30c", "AQAAAAIAAYagAAAAEILyjao5ghR9Xei+gcuC5fBEVNaffXTUByw9GrrgMqNWuoOypjL9l25vROqQn5xTzQ==", "c124b815-cbb2-408a-b5f0-bff41b84b782" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "59657b65-513c-42f7-8bd6-65afefded355", "AQAAAAIAAYagAAAAENc+muLToCnYIHiqyaXsayA8Hfjud+e8KmnVpaIQmBIuTKouNNVR/gB8gpVSynxnzw==", "d9f9d690-5c10-4bea-8b06-7149343fbff4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94648935-a7d4-4484-b11e-3d5120ec6b53", "AQAAAAIAAYagAAAAEAwtr31b3GOAFG5FKKA33eedZjnVgs1SH06dRA5fhM0T1OXlgJlljjt1doLs4C3FBg==", "d59081fa-4dd6-4578-913b-776da5a5f464" });
        }
    }
}
