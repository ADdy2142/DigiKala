using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectDigiKala.Migrations
{
    public partial class AddPaymentTypeToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "Orders",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");
        }
    }
}
