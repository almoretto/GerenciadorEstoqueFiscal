using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagerCore.Migrations
{
    public partial class NewFieldNfControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneratorProposals",
                table: "NFControls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratorProposals",
                table: "NFControls");
        }
    }
}
