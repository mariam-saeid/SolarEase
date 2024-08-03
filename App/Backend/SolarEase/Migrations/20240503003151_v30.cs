using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityTariffSubranges");

            migrationBuilder.DropTable(
                name: "ElectricityTariffs");

            migrationBuilder.AddColumn<double>(
                name: "saved",
                table: "Calculators",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ElectricityConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartRange = table.Column<int>(type: "int", nullable: false),
                    EndRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityConsumptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityConsumptionSubranges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartRange = table.Column<int>(type: "int", nullable: false),
                    EndRange = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ElectricityConsumptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityConsumptionSubranges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityConsumptionSubranges_ElectricityConsumptions_ElectricityConsumptionId",
                        column: x => x.ElectricityConsumptionId,
                        principalTable: "ElectricityConsumptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityConsumptionSubranges_ElectricityConsumptionId",
                table: "ElectricityConsumptionSubranges",
                column: "ElectricityConsumptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityConsumptionSubranges");

            migrationBuilder.DropTable(
                name: "ElectricityConsumptions");

            migrationBuilder.DropColumn(
                name: "saved",
                table: "Calculators");

            migrationBuilder.CreateTable(
                name: "ElectricityTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndRange = table.Column<int>(type: "int", nullable: false),
                    StartRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityTariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityTariffSubranges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectricityTariffId = table.Column<int>(type: "int", nullable: false),
                    EndRange = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    StartRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityTariffSubranges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityTariffSubranges_ElectricityTariffs_ElectricityTariffId",
                        column: x => x.ElectricityTariffId,
                        principalTable: "ElectricityTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityTariffSubranges_ElectricityTariffId",
                table: "ElectricityTariffSubranges",
                column: "ElectricityTariffId");
        }
    }
}
