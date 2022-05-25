using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class editorderisincludedeleveryprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncludeDeliveryPrice",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncludeDeliveryPrice",
                table: "Order");
        }
    }
}
