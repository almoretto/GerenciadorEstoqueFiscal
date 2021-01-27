using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class amountbalancefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductBalance",
                table: "Stocks");

            migrationBuilder.AddColumn<double>(
                name: "ProdAmountBalance",
                table: "Stocks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ProdQtyBalance",
                table: "Stocks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdAmountBalance",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ProdQtyBalance",
                table: "Stocks");

            migrationBuilder.AddColumn<int>(
                name: "ProductBalance",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
