using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectDigiKala.Migrations
{
    public partial class RemovePriceFromCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);
        }
    }
}
