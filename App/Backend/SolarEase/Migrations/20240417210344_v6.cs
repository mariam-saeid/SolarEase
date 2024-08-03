using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SolarProducts");

            migrationBuilder.DropColumn(
                name: "Favorited",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SolarProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Favorited",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
