using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class v81CorretionStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Stocks_StockId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "InputProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "SoldProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "InputProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Stocks_StockId",
                table: "SoldProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
