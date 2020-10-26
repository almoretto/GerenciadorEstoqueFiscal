using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class V4AlterRelationProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Products_ProductId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Products_ProductId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_ProductId",
                table: "InputProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SoldProducts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InputProducts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_ProductId",
                table: "InputProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Products_ProductId",
                table: "InputProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Products_ProductId",
                table: "SoldProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputProducts_Products_ProductId",
                table: "InputProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Products_ProductId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProducts");

            migrationBuilder.DropIndex(
                name: "IX_InputProducts_ProductId",
                table: "InputProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SoldProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InputProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProducts",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputProducts_ProductId",
                table: "InputProducts",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InputProducts_Products_ProductId",
                table: "InputProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Products_ProductId",
                table: "SoldProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
