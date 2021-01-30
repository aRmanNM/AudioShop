using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddedOrderEpisodeAndBlacklistItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Orders_OrderId",
                table: "Episodes");

            migrationBuilder.DropIndex(
                name: "IX_Episodes_OrderId",
                table: "Episodes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CouponId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Episodes");

            migrationBuilder.AddColumn<int>(
                name: "CouponId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blacklist",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    CouponId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blacklist", x => new { x.CouponId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Blacklist_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blacklist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderEpisodes",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false),
                    EpisodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEpisodes", x => new { x.EpisodeId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderEpisodes_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderEpisodes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CouponId1",
                table: "AspNetUsers",
                column: "CouponId1");

            migrationBuilder.CreateIndex(
                name: "IX_Blacklist_UserId",
                table: "Blacklist",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEpisodes_OrderId",
                table: "OrderEpisodes",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId1",
                table: "AspNetUsers",
                column: "CouponId1",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId1",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Blacklist");

            migrationBuilder.DropTable(
                name: "OrderEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CouponId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CouponId1",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Episodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_OrderId",
                table: "Episodes",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CouponId",
                table: "AspNetUsers",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Coupons_CouponId",
                table: "AspNetUsers",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Orders_OrderId",
                table: "Episodes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
