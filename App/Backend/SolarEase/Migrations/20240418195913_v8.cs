using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "SolarProducts",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "IsPostProduct",
                table: "SolarProducts",
                newName: "IsProductPost");

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => new { x.PersonId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_SolarProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "SolarProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.RenameColumn(
                name: "IsProductPost",
                table: "SolarProducts",
                newName: "IsPostProduct");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "SolarProducts",
                newName: "PhotoUrl");
        }
    }
}
