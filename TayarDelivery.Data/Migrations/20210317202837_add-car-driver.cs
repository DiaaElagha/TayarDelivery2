using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class addcardriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "DriverCarModel",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverCarNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverCarType",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverCarModel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DriverCarNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DriverCarType",
                table: "AspNetUsers");
        }
    }
}
