using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class StockAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalcDate",
                table: "Stocks");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInput",
                table: "Stocks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSales",
                table: "Stocks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastInput",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "LastSales",
                table: "Stocks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CalcDate",
                table: "Stocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
