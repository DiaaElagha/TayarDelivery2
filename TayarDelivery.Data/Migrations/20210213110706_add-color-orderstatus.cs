using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class addcolororderstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "OrderStatus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSetDriver",
                table: "Order",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "IsSetDriver",
                table: "Order");
        }
    }
}
