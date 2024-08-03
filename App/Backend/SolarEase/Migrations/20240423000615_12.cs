using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class _12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnvironmentalBenefit",
                table: "Calculators",
                newName: "EnvironmentalBenefitYearly");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "EnvironmentalBenefitMonthly",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EnvironmentalBenefitTwentyFiveYear",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "EnvironmentalBenefitMonthly",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "EnvironmentalBenefitTwentyFiveYear",
                table: "Calculators");

            migrationBuilder.RenameColumn(
                name: "EnvironmentalBenefitYearly",
                table: "Calculators",
                newName: "EnvironmentalBenefit");
        }
    }
}
