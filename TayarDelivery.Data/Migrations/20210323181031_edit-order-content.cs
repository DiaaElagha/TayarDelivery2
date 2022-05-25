using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class editordercontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderContentId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportEmail",
                table: "CompanyInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportNumber",
                table: "CompanyInformation",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderContent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderContent_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderContent_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderContentId",
                table: "Order",
                column: "OrderContentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderContent_CreateByUserId",
                table: "OrderContent",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderContent_UpdateByUserId",
                table: "OrderContent",
                column: "UpdateByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderContent_OrderContentId",
                table: "Order",
                column: "OrderContentId",
                principalTable: "OrderContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderContent_OrderContentId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "OrderContent");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderContentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderContentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SupportEmail",
                table: "CompanyInformation");

            migrationBuilder.DropColumn(
                name: "SupportNumber",
                table: "CompanyInformation");
        }
    }
}
