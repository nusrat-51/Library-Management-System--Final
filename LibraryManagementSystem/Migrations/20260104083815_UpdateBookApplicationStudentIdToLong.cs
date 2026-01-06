using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookApplicationStudentIdToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "501bf5ef-ec16-4540-a43a-89bbadd7a90b", "AQAAAAIAAYagAAAAEKQ1XV//8MOWbPf0OoRSznB/yNabUTRNKoZFntCKqRQftjVgiOqAqWCl3+4l/1F7TQ==", "8f50b1c0-0e83-475f-9b49-f519d8fcb987" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7c81d3e-9aa0-43dd-903d-e4dc9ab057d0", "AQAAAAIAAYagAAAAELjhdEYPOg7NMEB0oFdcf7Ai2+BXHXQ67fGvalwzShbBTQK8yVKrK9bYf/pr7B6vCA==", "8dcb98b7-41b9-4f31-8a2a-f6df0f928de5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32294296-471d-4892-8088-54eb6d7c2160", "AQAAAAIAAYagAAAAEBLc8LIAFhDYdrwudcnZxYZpLXGVO6wM+pO8SZJzmrXVkxy5QhT9M22d29ttJ5i7qg==", "bab89c28-3d73-4a00-a1de-7dbc9f37d6c6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
