using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RezervationApp.Migrations
{
    /// <inheritdoc />
    public partial class UserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "UserType",
                table: "Users",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid", "UserType" },
                values: new object[] { new DateTime(2025, 11, 10, 21, 25, 51, 338, DateTimeKind.Local).AddTicks(9077), new Guid("13f691e8-6ad9-4b57-bb6b-9a36f0129d3b"), (byte)0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 11, 3, 22, 43, 9, 824, DateTimeKind.Local).AddTicks(5036), null });
        }
    }
}
