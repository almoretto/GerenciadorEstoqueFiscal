using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace StockManagerCore.Migrations
{
    public partial class v7StockModel_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SoldProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "SoldProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "InputProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "InputProducts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: true),
                    QtyPurchased = table.Column<int>(nullable: false),
                    QtySold = table.Column<int>(nullable: false),
                    AmountPurchased = table.Column<double>(nullable: false),
                    AmountSold = table.Column<double>(nullable: false),
                    CalcDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "ATACADAO" });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "JR" });

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_CompanyId",
                table: "SoldProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_CompanyId",
                table: "InputProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CompanyId",
                table: "Stocks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_companies_CompanyId",
                table: "InputProducts",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_companies_CompanyId",
                table: "SoldProducts",
                column: "CompanyId",
                principalTable: "companies",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_companies_CompanyId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Stocks_StockId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_companies_CompanyId",
                table: "SoldProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Stocks_StockId",
                table: "SoldProducts");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_CompanyId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_StockId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_CompanyId",
                table: "InputProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_StockId",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "InputProducts");
        }
    }
}
