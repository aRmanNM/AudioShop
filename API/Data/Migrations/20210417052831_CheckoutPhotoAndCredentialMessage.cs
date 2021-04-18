using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class CheckoutPhotoAndCredentialMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "SalespersonCredentials",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptPhotoId",
                table: "Checkouts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_ReceiptPhotoId",
                table: "Checkouts",
                column: "ReceiptPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Photos_ReceiptPhotoId",
                table: "Checkouts",
                column: "ReceiptPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Photos_ReceiptPhotoId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_ReceiptPhotoId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "SalespersonCredentials");

            migrationBuilder.DropColumn(
                name: "ReceiptPhotoId",
                table: "Checkouts");
        }
    }
}
