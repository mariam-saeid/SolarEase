using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RoofSpace",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoofSpace",
                table: "Calculators");
        }
    }
}
