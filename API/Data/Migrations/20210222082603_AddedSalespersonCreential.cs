using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddedSalespersonCreential : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalespersonCredentialId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalespersonCredentialId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalespersonCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    IdCardNumber = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankAccountShebaNumber = table.Column<string>(nullable: true),
                    BankCardNumber = table.Column<string>(nullable: true),
                    BankCardName = table.Column<string>(nullable: true),
                    IdCardPhotoId = table.Column<int>(nullable: true),
                    BankCardPhotoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalespersonCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalespersonCredentials_Photos_BankCardPhotoId",
                        column: x => x.BankCardPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalespersonCredentials_Photos_IdCardPhotoId",
                        column: x => x.IdCardPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SalespersonCredentialId1",
                table: "AspNetUsers",
                column: "SalespersonCredentialId1");

            migrationBuilder.CreateIndex(
                name: "IX_SalespersonCredentials_BankCardPhotoId",
                table: "SalespersonCredentials",
                column: "BankCardPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_SalespersonCredentials_IdCardPhotoId",
                table: "SalespersonCredentials",
                column: "IdCardPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SalespersonCredentials_SalespersonCredentialId1",
                table: "AspNetUsers",
                column: "SalespersonCredentialId1",
                principalTable: "SalespersonCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SalespersonCredentials_SalespersonCredentialId1",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SalespersonCredentials");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SalespersonCredentialId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalespersonCredentialId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalespersonCredentialId1",
                table: "AspNetUsers");
        }
    }
}
