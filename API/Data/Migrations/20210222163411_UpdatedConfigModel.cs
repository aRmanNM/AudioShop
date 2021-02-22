using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class UpdatedConfigModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupEn",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupFa",
                table: "Configs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupEn",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "GroupFa",
                table: "Configs");
        }
    }
}
