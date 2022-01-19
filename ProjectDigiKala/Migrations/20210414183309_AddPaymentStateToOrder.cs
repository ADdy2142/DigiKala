using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectDigiKala.Migrations
{
    public partial class AddPaymentStateToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentState",
                table: "Orders",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentState",
                table: "Orders");
        }
    }
}
