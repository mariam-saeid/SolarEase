using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saved",
                table: "Calculators");

            migrationBuilder.AddColumn<string>(
                name: "FinancialSavingMonthly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinancialSavingTwentyFiveYear",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinancialSavingYearly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialSavingMonthly",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "FinancialSavingTwentyFiveYear",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "FinancialSavingYearly",
                table: "Calculators");

            migrationBuilder.AddColumn<double>(
                name: "saved",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
