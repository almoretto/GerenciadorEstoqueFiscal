using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class V5AlterRelationStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Stocks_StockId1",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Stocks_StockId",
                table: "SoldProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Stocks_StockId1",
                table: "SoldProducts");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_StockId1",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_StockId1",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "StockId1",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "StockId1",
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
                name: "StockId1",
                table: "SoldProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "InputProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId1",
                table: "InputProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_StockId1",
                table: "SoldProducts",
                column: "StockId1");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_StockId1",
                table: "InputProducts",
                column: "StockId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Stocks_StockId1",
                table: "InputProducts",
                column: "StockId1",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Stocks_StockId1",
                table: "SoldProducts",
                column: "StockId1",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
