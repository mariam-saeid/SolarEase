using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v46 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Admins");
        }
    }
}
