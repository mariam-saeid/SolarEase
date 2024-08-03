using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CyclicalDailyPath",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DailyPath",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "HourlyPath",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "ScalerDailyPath",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "cyclicalHourlyPath",
                table: "Cities",
                newName: "HourlyUrl");

            migrationBuilder.RenameColumn(
                name: "ScalerHourlyPath",
                table: "Cities",
                newName: "DailyUrl");

            migrationBuilder.AddColumn<string>(
                name: "MeasuringUnit",
                table: "SolarProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasuringUnit",
                table: "SolarProducts");

            migrationBuilder.RenameColumn(
                name: "HourlyUrl",
                table: "Cities",
                newName: "cyclicalHourlyPath");

            migrationBuilder.RenameColumn(
                name: "DailyUrl",
                table: "Cities",
                newName: "ScalerHourlyPath");

            migrationBuilder.AddColumn<string>(
                name: "CyclicalDailyPath",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DailyPath",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HourlyPath",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Latitude",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Longitude",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ScalerDailyPath",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
