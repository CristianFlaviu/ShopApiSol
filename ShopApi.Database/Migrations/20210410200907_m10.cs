using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.Database.Migrations
{
    public partial class m10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseUserProduct",
                columns: table => new
                {
                    FavoriteUsersId = table.Column<string>(type: "text", nullable: false),
                    FavoritesProductsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserProduct", x => new { x.FavoriteUsersId, x.FavoritesProductsId });
                    table.ForeignKey(
                        name: "FK_BaseUserProduct_AspNetUsers_FavoriteUsersId",
                        column: x => x.FavoriteUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserProduct_Products_FavoritesProductsId",
                        column: x => x.FavoritesProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserProduct1",
                columns: table => new
                {
                    ShoppingCartProductsId = table.Column<int>(type: "integer", nullable: false),
                    ShoppingCartUsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserProduct1", x => new { x.ShoppingCartProductsId, x.ShoppingCartUsersId });
                    table.ForeignKey(
                        name: "FK_BaseUserProduct1_AspNetUsers_ShoppingCartUsersId",
                        column: x => x.ShoppingCartUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserProduct1_Products_ShoppingCartProductsId",
                        column: x => x.ShoppingCartProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserProduct_FavoritesProductsId",
                table: "BaseUserProduct",
                column: "FavoritesProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserProduct1_ShoppingCartUsersId",
                table: "BaseUserProduct1",
                column: "ShoppingCartUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseUserProduct");

            migrationBuilder.DropTable(
                name: "BaseUserProduct1");
        }
    }
}
