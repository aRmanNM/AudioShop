using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AdFileAndLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Ads",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_FileId",
                table: "Ads",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_ContentFiles_FileId",
                table: "Ads",
                column: "FileId",
                principalTable: "ContentFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_ContentFiles_FileId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_FileId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Ads");
        }
    }
}
