using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PadiScanner.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    OriginalImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeatmapImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverlayedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClippedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Probabilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploaderId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_Users_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "LastLoginAt", "Password", "Role", "Username" },
                values: new object[] { "01GE24HFHHQZRRN024W32W8XF7", "NyankoAdmin", new DateTime(2022, 10, 2, 13, 42, 59, 74, DateTimeKind.Local).AddTicks(6877), "$2a$11$QvQADnPMFBKLgGQKCuxcNO8Ppvde0KO.6KQn/PmX8ypC4/rO3F/om", 0, "nynanko" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "LastLoginAt", "Password", "Role", "Username" },
                values: new object[] { "01GE24MT8165ZNXACDZYC8GMEQ", "Tamu", new DateTime(2022, 10, 2, 13, 42, 59, 236, DateTimeKind.Local).AddTicks(6398), "$2a$11$qJQdZBMIILVJvXNEEk4fT./LLmmCzt3yeXZUe5OXINe9Dh0duj/.q", 2, "tamu" });

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_UploaderId",
                table: "Predictions",
                column: "UploaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
