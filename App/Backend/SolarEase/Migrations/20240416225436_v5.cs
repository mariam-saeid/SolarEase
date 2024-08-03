using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "SolarProducts",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "IsProductPost",
                table: "SolarProducts",
                newName: "IsPostProduct");

            migrationBuilder.AddColumn<bool>(
                name: "Favorited",
                table: "SolarProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Favorited",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorited",
                table: "SolarProducts");

            migrationBuilder.DropColumn(
                name: "Favorited",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "SolarProducts",
                newName: "Photo");

            migrationBuilder.RenameColumn(
                name: "IsPostProduct",
                table: "SolarProducts",
                newName: "IsProductPost");
        }
    }
}
