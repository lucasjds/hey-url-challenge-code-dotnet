using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hey_url_challenge_code_dotnet.Migrations
{
    public partial class AddNewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Urls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "VisitLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUrl = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plataform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClickDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitLogs_Urls_IdUrl",
                        column: x => x.IdUrl,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitLogs_IdUrl",
                table: "VisitLogs",
                column: "IdUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                table: "Urls");
        }
    }
}
