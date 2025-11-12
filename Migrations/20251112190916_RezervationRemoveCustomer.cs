using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RezervationApp.Migrations
{
    /// <inheritdoc />
    public partial class RezervationRemoveCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervations_Customers_CustomerId",
                table: "Rezervations");

            migrationBuilder.DropIndex(
                name: "IX_Rezervations_CustomerId",
                table: "Rezervations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Rezervations");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 22, 9, 15, 566, DateTimeKind.Local).AddTicks(6825), new Guid("0c82bbfc-c3e5-4b11-8ac5-7110dfc605e9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Rezervations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 11, 10, 21, 25, 51, 338, DateTimeKind.Local).AddTicks(9077), new Guid("13f691e8-6ad9-4b57-bb6b-9a36f0129d3b") });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervations_CustomerId",
                table: "Rezervations",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervations_Customers_CustomerId",
                table: "Rezervations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
