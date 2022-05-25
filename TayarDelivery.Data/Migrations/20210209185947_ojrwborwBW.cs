using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class ojrwborwBW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "HomeInfo");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageHeight",
                table: "HomeInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageWidth",
                table: "HomeInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainTitle",
                table: "HomeInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainTitleColor",
                table: "HomeInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "HomeInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitleColor",
                table: "HomeInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImageHeight",
                table: "HomeInfo");

            migrationBuilder.DropColumn(
                name: "BackgroundImageWidth",
                table: "HomeInfo");

            migrationBuilder.DropColumn(
                name: "MainTitle",
                table: "HomeInfo");

            migrationBuilder.DropColumn(
                name: "MainTitleColor",
                table: "HomeInfo");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "HomeInfo");

            migrationBuilder.DropColumn(
                name: "SubTitleColor",
                table: "HomeInfo");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HomeInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
