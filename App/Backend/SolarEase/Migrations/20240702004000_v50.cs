using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQMessages",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    FAQId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_FAQMessages_FAQId",
                table: "FAQMessages",
                column: "FAQId");
        }
    }
}
