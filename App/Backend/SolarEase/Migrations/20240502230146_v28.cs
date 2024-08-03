using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatteryCapacity",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatteryPrice",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumofBatteries",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TotalCostStr",
                table: "Calculators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ElectricityTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartRange = table.Column<int>(type: "int", nullable: false),
                    EndRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityTariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityTariffSubrange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartRange = table.Column<int>(type: "int", nullable: false),
                    EndRange = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ElectricityTariffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityTariffSubrange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityTariffSubrange_ElectricityTariffs_ElectricityTariffId",
                        column: x => x.ElectricityTariffId,
                        principalTable: "ElectricityTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityTariffSubrange_ElectricityTariffId",
                table: "ElectricityTariffSubrange",
                column: "ElectricityTariffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityTariffSubrange");

            migrationBuilder.DropTable(
                name: "ElectricityTariffs");

            migrationBuilder.DropColumn(
                name: "BatteryCapacity",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "BatteryPrice",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "NumofBatteries",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Calculators");

            migrationBuilder.DropColumn(
                name: "TotalCostStr",
                table: "Calculators");
        }
    }
}
