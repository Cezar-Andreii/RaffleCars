using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoRaffleBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDrawingTimeToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BuyerName",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "DrawingTime",
                table: "Cars",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrawingTime",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerName",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
