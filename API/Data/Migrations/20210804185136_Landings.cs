using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class Landings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Landings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogoId = table.Column<int>(nullable: true),
                    LogoEnabled = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    TitleEnabled = table.Column<bool>(nullable: false),
                    MediaId = table.Column<int>(nullable: true),
                    MediaEnabled = table.Column<bool>(nullable: false),
                    Text1 = table.Column<string>(nullable: true),
                    Text1Enabled = table.Column<bool>(nullable: false),
                    Button = table.Column<string>(nullable: true),
                    ButtonLink = table.Column<string>(nullable: true),
                    ButtonEnabled = table.Column<bool>(nullable: false),
                    Text2 = table.Column<string>(nullable: true),
                    Text2Enabled = table.Column<bool>(nullable: false),
                    Gift = table.Column<string>(nullable: true),
                    GiftEnabled = table.Column<bool>(nullable: false),
                    PhoneBoxEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Landings_Photos_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Landings_ContentFiles_MediaId",
                        column: x => x.MediaId,
                        principalTable: "ContentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LandingPhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandingId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingPhoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandingPhoneNumbers_Landings_LandingId",
                        column: x => x.LandingId,
                        principalTable: "Landings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandingPhoneNumbers_LandingId",
                table: "LandingPhoneNumbers",
                column: "LandingId");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_LogoId",
                table: "Landings",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_MediaId",
                table: "Landings",
                column: "MediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandingPhoneNumbers");

            migrationBuilder.DropTable(
                name: "Landings");

            migrationBuilder.DropTable(
                name: "ContentFiles");
        }
    }
}
