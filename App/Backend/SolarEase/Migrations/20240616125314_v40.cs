using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedFilter",
                table: "Admins");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "SolarInstallers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "SolarInstallers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "SolarInstallers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "SolarInstallers");

            migrationBuilder.AddColumn<bool>(
                name: "AllowedFilter",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
