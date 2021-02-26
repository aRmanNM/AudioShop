using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddedTimeBetweenEpisodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "SalespersonCredentials",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingTimeBetweenEpisodes",
                table: "Courses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "SalespersonCredentials");

            migrationBuilder.DropColumn(
                name: "WaitingTimeBetweenEpisodes",
                table: "Courses");
        }
    }
}
