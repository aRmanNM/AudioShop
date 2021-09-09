using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class MessageUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "ClockRangeBegin",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ClockRangeEnd",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "InAppSeen",
                table: "UserMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushSent",
                table: "UserMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SMSSent",
                table: "UserMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RepeatAfterHour",
                table: "Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SendInApp",
                table: "Messages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InAppSeen",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "PushSent",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "SMSSent",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "RepeatAfterHour",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SendInApp",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClockRangeBegin",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClockRangeEnd",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
