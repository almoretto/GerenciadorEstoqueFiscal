using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class v10InitialNewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NItem = table.Column<string>(nullable: true),
                    XProd = table.Column<string>(nullable: true),
                    QCom = table.Column<int>(nullable: false),
                    VUnCom = table.Column<double>(nullable: false),
                    UCom = table.Column<string>(nullable: true),
                    Vtotal = table.Column<double>(nullable: false),
                    VUnTrib = table.Column<double>(nullable: false),
                    VTotTrib = table.Column<double>(nullable: false),
                    DhEmi = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputProducts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoldProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NItem = table.Column<string>(nullable: true),
                    XProd = table.Column<string>(nullable: true),
                    QCom = table.Column<int>(nullable: false),
                    VUnCom = table.Column<double>(nullable: false),
                    Vtotal = table.Column<double>(nullable: false),
                    DhEmi = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_Stocks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_CompanyId",
                table: "InputProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_ProductId",
                table: "InputProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_CompanyId",
                table: "SoldProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CompanyId",
                table: "Stocks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputProducts");

            migrationBuilder.DropTable(
                name: "SoldProducts");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
