using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class v8StockModel_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_companies_CompanyId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_companies_CompanyId",
                table: "SoldProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_companies_CompanyId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_companies",
                table: "companies");

            migrationBuilder.RenameTable(
                name: "companies",
                newName: "Companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Companies_CompanyId",
                table: "InputProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Companies_CompanyId",
                table: "SoldProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Companies_CompanyId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Companies_CompanyId",
                table: "SoldProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_companies",
                table: "companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_companies_CompanyId",
                table: "InputProducts",
                column: "CompanyId",
                principalTable: "companies",
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
                name: "FK_Stocks_companies_CompanyId",
                table: "Stocks",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
