using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEase.Migrations
{
    /// <inheritdoc />
    public partial class v27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoritePosts_Persons_PersonId",
                table: "FavoritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoritePosts_Posts_PostId",
                table: "FavoritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Persons_PersonId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_SolarProducts_ProductId",
                table: "FavoriteProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritePosts_Persons_PersonId",
                table: "FavoritePosts",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritePosts_Posts_PostId",
                table: "FavoritePosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Persons_PersonId",
                table: "FavoriteProducts",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_SolarProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId",
                principalTable: "SolarProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoritePosts_Persons_PersonId",
                table: "FavoritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoritePosts_Posts_PostId",
                table: "FavoritePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Persons_PersonId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_SolarProducts_ProductId",
                table: "FavoriteProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritePosts_Persons_PersonId",
                table: "FavoritePosts",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritePosts_Posts_PostId",
                table: "FavoritePosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Persons_PersonId",
                table: "FavoriteProducts",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_SolarProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId",
                principalTable: "SolarProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
