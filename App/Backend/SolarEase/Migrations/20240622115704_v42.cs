using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatbotHistoryMessages",
                table: "ChatbotHistoryMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatbotHistoryMessages",
                table: "ChatbotHistoryMessages",
                columns: new[] { "PersonId", "FAQId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatbotHistoryMessages",
                table: "ChatbotHistoryMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatbotHistoryMessages",
                table: "ChatbotHistoryMessages",
                columns: new[] { "PersonId", "FAQId" });
        }
    }
}
