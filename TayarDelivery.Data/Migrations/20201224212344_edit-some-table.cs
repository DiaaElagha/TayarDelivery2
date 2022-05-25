using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class editsometable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AdditionalCost",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DiscountedCost",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensedNumber",
                table: "CompanyInformation",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DiscountPriceWhenReturn",
                table: "AreasPrice",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillTahsil",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<float>(nullable: true),
                    NumberOfOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillTahsil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillTahsil_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillTahsil_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistory_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderHistory_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_CreateByUserId",
                table: "BillTahsil",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_UpdateByUserId",
                table: "BillTahsil",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_CreateByUserId",
                table: "OrderHistory",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_OrderId",
                table: "OrderHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_UpdateByUserId",
                table: "OrderHistory",
                column: "UpdateByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillTahsil");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "AdditionalCost",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountedCost",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LicensedNumber",
                table: "CompanyInformation");

            migrationBuilder.DropColumn(
                name: "DiscountPriceWhenReturn",
                table: "AreasPrice");
        }
    }
}
