using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce1.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "CartDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
