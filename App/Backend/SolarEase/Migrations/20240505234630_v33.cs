using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumofPanels",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "PanelsCapacity",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "PanelsPrice",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "SystemSizeStr",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "TotalCostStr",
                table: "Calculators");

            migrationBuilder.AlterColumn<double>(
                name: "PaybackPeriod",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "NumofBatteries",
                table: "Calculators",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "InverterPrice",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "InverterCapacity",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "FinancialSavingYearly",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "FinancialSavingTwentyFiveYear",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "FinancialSavingMonthly",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "EnvironmentalBenefitYearly",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "EnvironmentalBenefitTwentyFiveYear",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "EnvironmentalBenefitMonthly",
                table: "Calculators",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "BatteryPrice",
                table: "Calculators",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BatteryCapacity",
                table: "Calculators",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxNumofPanels",
                table: "Calculators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxPanelsCapacity",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxPanelsPrice",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MiniNumofPanels",
                table: "Calculators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MiniPanelsCapacity",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MiniPanelsPrice",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxNumofPanels",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "MaxPanelsCapacity",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "MaxPanelsPrice",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "MiniNumofPanels",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "MiniPanelsCapacity",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "MiniPanelsPrice",
                table: "Calculators");

            migrationBuilder.AlterColumn<string>(
                name: "PaybackPeriod",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "NumofBatteries",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InverterPrice",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "InverterCapacity",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialSavingYearly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialSavingTwentyFiveYear",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialSavingMonthly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "EnvironmentalBenefitYearly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "EnvironmentalBenefitTwentyFiveYear",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "EnvironmentalBenefitMonthly",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "BatteryPrice",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BatteryCapacity",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumofPanels",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PanelsCapacity",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PanelsPrice",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SystemSizeStr",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalCostStr",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
