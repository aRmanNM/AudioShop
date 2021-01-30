using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ChangedCouponFunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Orders_OrderId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_OrderId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CouponId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CouponId1",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "OtherCouponCode",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalespersonCouponCode",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherCouponCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SalespersonCouponCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouponId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_OrderId",
                table: "Coupons",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CouponId1",
                table: "AspNetUsers",
                column: "CouponId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId1",
                table: "AspNetUsers",
                column: "CouponId1",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Orders_OrderId",
                table: "Coupons",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
