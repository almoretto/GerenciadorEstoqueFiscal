using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class v6AlterModelSoldProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UCom",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "VTotTrib",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "VUnTrib",
                table: "SoldProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UCom",
                table: "SoldProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VTotTrib",
                table: "SoldProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VUnTrib",
                table: "SoldProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
