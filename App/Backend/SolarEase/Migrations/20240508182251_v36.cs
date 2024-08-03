using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyEnergyPredictions");

            migrationBuilder.DropTable(
                name: "HourlyEnergyPredictions");

            migrationBuilder.CreateTable(
                name: "DailyAllSkyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PredictedAllSky = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAllSkyPredictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HourlyAllSkyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PredictedAllSky = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyAllSkyPredictions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyAllSkyPredictions");

            migrationBuilder.DropTable(
                name: "HourlyAllSkyPredictions");

            migrationBuilder.CreateTable(
                name: "DailyEnergyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    EnergyPrediction = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEnergyPredictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HourlyEnergyPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    EnergyPrediction = table.Column<double>(type: "float", nullable: false),
                    Hour = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyEnergyPredictions", x => x.Id);
                });
        }
    }
}
