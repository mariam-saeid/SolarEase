using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "FAQs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ChatbotHistoryMessages",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    FAQId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatbotHistoryMessages", x => new { x.PersonId, x.FAQId });
                    table.ForeignKey(
                        name: "FK_ChatbotHistoryMessages_FAQs_FAQId",
                        column: x => x.FAQId,
                        principalTable: "FAQs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatbotHistoryMessages_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatbotHistoryMessages_FAQId",
                table: "ChatbotHistoryMessages",
                column: "FAQId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatbotHistoryMessages");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "FAQs");
        }
    }
}
