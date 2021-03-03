using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class FixedCredentialRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SalespersonCredentials_SalespersonCredentialId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SalespersonCredentialId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalespersonCredentialId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalespersonCredentialId1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SalespersonCredentials",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalespersonCredentials_UserId",
                table: "SalespersonCredentials",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SalespersonCredentials_AspNetUsers_UserId",
                table: "SalespersonCredentials",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalespersonCredentials_AspNetUsers_UserId",
                table: "SalespersonCredentials");

            migrationBuilder.DropIndex(
                name: "IX_SalespersonCredentials_UserId",
                table: "SalespersonCredentials");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SalespersonCredentials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalespersonCredentialId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalespersonCredentialId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SalespersonCredentialId1",
                table: "AspNetUsers",
                column: "SalespersonCredentialId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SalespersonCredentials_SalespersonCredentialId1",
                table: "AspNetUsers",
                column: "SalespersonCredentialId1",
                principalTable: "SalespersonCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
