using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v49 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQHistoryMessages");

            migrationBuilder.CreateTable(
                name: "ChatbotMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatbotMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatbotMessages_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FAQMessages",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    FAQId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQMessages", x => new { x.PersonId, x.FAQId, x.Date });
                    table.ForeignKey(
                        name: "FK_FAQMessages_FAQs_FAQId",
                        column: x => x.FAQId,
                        principalTable: "FAQs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FAQMessages_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatbotMessages_PersonId",
                table: "ChatbotMessages",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQMessages_FAQId",
                table: "FAQMessages",
                column: "FAQId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatbotMessages");

            migrationBuilder.DropTable(
                name: "FAQMessages");

            migrationBuilder.CreateTable(
                name: "FAQHistoryMessages",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    FAQId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQHistoryMessages", x => new { x.PersonId, x.FAQId, x.Date });
                    table.ForeignKey(
                        name: "FK_FAQHistoryMessages_FAQs_FAQId",
                        column: x => x.FAQId,
                        principalTable: "FAQs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FAQHistoryMessages_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQHistoryMessages_FAQId",
                table: "FAQHistoryMessages",
                column: "FAQId");
        }
    }
}
