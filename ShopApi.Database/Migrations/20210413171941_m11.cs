using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShopApi.Database.Migrations
{
    public partial class m11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseUserProduct");

            migrationBuilder.DropTable(
                name: "BaseUserProduct1");

            migrationBuilder.CreateTable(
                name: "ProductsUsersShopping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsUsersShopping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsUsersShopping_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductsUsersShopping_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsUsersShopping_ProductId",
                table: "ProductsUsersShopping",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsUsersShopping_UserId",
                table: "ProductsUsersShopping",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsUsersShopping");

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
    }
}
