using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class ModelV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SQty",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "SUnValue",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "IQty",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "UnValue",
                table: "InputProducts");

            migrationBuilder.AddColumn<string>(
                name: "NItem",
                table: "SoldProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QCom",
                table: "SoldProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UCom",
                table: "SoldProducts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VTotTrib",
                table: "SoldProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VUnCom",
                table: "SoldProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VUnTrib",
                table: "SoldProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Vtotal",
                table: "SoldProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "XProd",
                table: "SoldProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NItem",
                table: "InputProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QCom",
                table: "InputProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UCom",
                table: "InputProducts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VTotTrib",
                table: "InputProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VUnCom",
                table: "InputProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VUnTrib",
                table: "InputProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Vtotal",
                table: "InputProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "XProd",
                table: "InputProducts",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group" },
                values: new object[,]
                {
                    { 1, "Anel" },
                    { 2, "Argola" },
                    { 3, "Bracelete" },
                    { 4, "Brinco" },
                    { 5, "Choker" },
                    { 6, "Colar" },
                    { 7, "Corrente" },
                    { 8, "Pingente" },
                    { 9, "Pulseira" },
                    { 10, "Tornozeleira" },
                    { 11, "Peças" },
                    { 12, "Variados" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "NItem",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "QCom",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "UCom",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "VTotTrib",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "VUnCom",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "VUnTrib",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "Vtotal",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "XProd",
                table: "SoldProducts");

            migrationBuilder.DropColumn(
                name: "NItem",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "QCom",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "UCom",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "VTotTrib",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "VUnCom",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "VUnTrib",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "Vtotal",
                table: "InputProducts");

            migrationBuilder.DropColumn(
                name: "XProd",
                table: "InputProducts");

            migrationBuilder.AddColumn<int>(
                name: "SQty",
                table: "SoldProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SUnValue",
                table: "SoldProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IQty",
                table: "InputProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "UnValue",
                table: "InputProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
