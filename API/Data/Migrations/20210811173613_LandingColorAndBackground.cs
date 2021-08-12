using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class LandingColorAndBackground : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackgroundId",
                table: "Landings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonsColor",
                table: "Landings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Landings_BackgroundId",
                table: "Landings",
                column: "BackgroundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Landings_Photos_BackgroundId",
                table: "Landings",
                column: "BackgroundId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Landings_Photos_BackgroundId",
                table: "Landings");

            migrationBuilder.DropIndex(
                name: "IX_Landings_BackgroundId",
                table: "Landings");

            migrationBuilder.DropColumn(
                name: "BackgroundId",
                table: "Landings");

            migrationBuilder.DropColumn(
                name: "ButtonsColor",
                table: "Landings");
        }
    }
}
