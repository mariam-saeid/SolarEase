using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<int>(type: "int", nullable: false),
                    DailyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScalerDailyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CyclicalDailyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScalerHourlyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cyclicalHourlyPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
