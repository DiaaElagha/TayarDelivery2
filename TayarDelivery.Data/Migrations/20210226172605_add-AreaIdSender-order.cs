using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class addAreaIdSenderorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaIdSender",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_AreaIdSender",
                table: "Order",
                column: "AreaIdSender");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Area_AreaIdSender",
                table: "Order",
                column: "AreaIdSender",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Area_AreaIdSender",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_AreaIdSender",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AreaIdSender",
                table: "Order");
        }
    }
}
