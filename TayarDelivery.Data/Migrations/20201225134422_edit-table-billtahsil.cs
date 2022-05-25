using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class edittablebilltahsil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TraderId",
                table: "BillTahsil",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_TraderId",
                table: "BillTahsil",
                column: "TraderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillTahsil_AspNetUsers_TraderId",
                table: "BillTahsil",
                column: "TraderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillTahsil_AspNetUsers_TraderId",
                table: "BillTahsil");

            migrationBuilder.DropIndex(
                name: "IX_BillTahsil_TraderId",
                table: "BillTahsil");

            migrationBuilder.DropColumn(
                name: "TraderId",
                table: "BillTahsil");
        }
    }
}
