using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddedCourseNameToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SliderItems_Courses_CourseId",
                table: "SliderItems");

            migrationBuilder.DropIndex(
                name: "IX_SliderItems_CourseId",
                table: "SliderItems");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Reviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Reviews");

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
    }
}
