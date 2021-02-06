using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddedNavigationPropertyToSliderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SliderItems_CourseId",
                table: "SliderItems",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderItems_Courses_CourseId",
                table: "SliderItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SliderItems_Courses_CourseId",
                table: "SliderItems");

            migrationBuilder.DropIndex(
                name: "IX_SliderItems_CourseId",
                table: "SliderItems");
        }
    }
}
