using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectricityTariffSubrange_ElectricityTariffs_ElectricityTariffId",
                table: "ElectricityTariffSubrange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricityTariffSubrange",
                table: "ElectricityTariffSubrange");

            migrationBuilder.RenameTable(
                name: "ElectricityTariffSubrange",
                newName: "ElectricityTariffSubranges");

            migrationBuilder.RenameIndex(
                name: "IX_ElectricityTariffSubrange_ElectricityTariffId",
                table: "ElectricityTariffSubranges",
                newName: "IX_ElectricityTariffSubranges_ElectricityTariffId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricityTariffSubranges",
                table: "ElectricityTariffSubranges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectricityTariffSubranges_ElectricityTariffs_ElectricityTariffId",
                table: "ElectricityTariffSubranges",
                column: "ElectricityTariffId",
                principalTable: "ElectricityTariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectricityTariffSubranges_ElectricityTariffs_ElectricityTariffId",
                table: "ElectricityTariffSubranges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricityTariffSubranges",
                table: "ElectricityTariffSubranges");

            migrationBuilder.RenameTable(
                name: "ElectricityTariffSubranges",
                newName: "ElectricityTariffSubrange");

            migrationBuilder.RenameIndex(
                name: "IX_ElectricityTariffSubranges_ElectricityTariffId",
                table: "ElectricityTariffSubrange",
                newName: "IX_ElectricityTariffSubrange_ElectricityTariffId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricityTariffSubrange",
                table: "ElectricityTariffSubrange",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectricityTariffSubrange_ElectricityTariffs_ElectricityTariffId",
                table: "ElectricityTariffSubrange",
                column: "ElectricityTariffId",
                principalTable: "ElectricityTariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
