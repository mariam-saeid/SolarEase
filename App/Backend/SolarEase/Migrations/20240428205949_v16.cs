using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_hourlyEnergyPredictions",
                table: "hourlyEnergyPredictions");

            migrationBuilder.RenameTable(
                name: "hourlyEnergyPredictions",
                newName: "HourlyEnergyPredictions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HourlyEnergyPredictions",
                table: "HourlyEnergyPredictions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HourlyEnergyPredictions",
                table: "HourlyEnergyPredictions");

            migrationBuilder.RenameTable(
                name: "HourlyEnergyPredictions",
                newName: "hourlyEnergyPredictions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hourlyEnergyPredictions",
                table: "hourlyEnergyPredictions",
                column: "Id");
        }
    }
}
