using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyEnergyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyPrediction = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEnergyPredictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hourlyEnergyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyPrediction = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hourlyEnergyPredictions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyEnergyPredictions");

            migrationBuilder.DropTable(
                name: "hourlyEnergyPredictions");
        }
    }
}
