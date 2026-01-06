using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixFinePaymentBookApplicationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
