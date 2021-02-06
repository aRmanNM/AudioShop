using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class ChangedNamesAndAddedSliderItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "SalePercentage",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TotalSales",
                table: "AspNetUsers",
                newName: "TotalSalesOfSalesperson");

            migrationBuilder.RenameColumn(
                name: "CurrentSales",
                table: "AspNetUsers",
                newName: "CurrentSalesOfSalesperson");

            migrationBuilder.AddColumn<int>(
                name: "TotalAudiosDuration",
                table: "Episodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Blacklist",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Audios",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalePercentageOfSalesperson",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SliderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: false),
                    PhotoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SliderItems_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SliderItems_PhotoId",
                table: "SliderItems",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SliderItems");

            migrationBuilder.DropColumn(
                name: "TotalAudiosDuration",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Blacklist");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "SalePercentageOfSalesperson",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TotalSalesOfSalesperson",
                table: "AspNetUsers",
                newName: "TotalSales");

            migrationBuilder.RenameColumn(
                name: "CurrentSalesOfSalesperson",
                table: "AspNetUsers",
                newName: "CurrentSales");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Configs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Configs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalePercentage",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
