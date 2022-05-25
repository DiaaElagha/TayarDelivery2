using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class editnotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DriverApprovalStatus",
                table: "Order",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Notification",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Notification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OrderId",
                table: "Notification",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Order_OrderId",
                table: "Notification",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Order_OrderId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_OrderId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "DriverApprovalStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notification");
        }
    }
}
