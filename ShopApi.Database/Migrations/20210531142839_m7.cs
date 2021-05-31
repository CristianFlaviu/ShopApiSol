using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.Database.Migrations
{
    public partial class m7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUserFavorites_AspNetUsers_UserId",
                table: "ProductsUserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUserFavorites_Products_ProductId",
                table: "ProductsUserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUsersShopping_AspNetUsers_UserId",
                table: "ProductsUsersShopping");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUsersShopping_Products_ProductId",
                table: "ProductsUsersShopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsUsersShopping",
                table: "ProductsUsersShopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsUserFavorites",
                table: "ProductsUserFavorites");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "ProductsUsersShopping",
                newName: "ShoppingCartProducts");

            migrationBuilder.RenameTable(
                name: "ProductsUserFavorites",
                newName: "FavoriteProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsUsersShopping_UserId",
                table: "ShoppingCartProducts",
                newName: "IX_ShoppingCartProducts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsUsersShopping_ProductId",
                table: "ShoppingCartProducts",
                newName: "IX_ShoppingCartProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsUserFavorites_UserId",
                table: "FavoriteProducts",
                newName: "IX_FavoriteProducts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsUserFavorites_ProductId",
                table: "FavoriteProducts",
                newName: "IX_FavoriteProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartProducts",
                table: "ShoppingCartProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteProducts",
                table: "FavoriteProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_AspNetUsers_UserId",
                table: "FavoriteProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Products_ProductId",
                table: "FavoriteProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartProducts_AspNetUsers_UserId",
                table: "ShoppingCartProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartProducts_Products_ProductId",
                table: "ShoppingCartProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_AspNetUsers_UserId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Products_ProductId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartProducts_AspNetUsers_UserId",
                table: "ShoppingCartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartProducts_Products_ProductId",
                table: "ShoppingCartProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartProducts",
                table: "ShoppingCartProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteProducts",
                table: "FavoriteProducts");

            migrationBuilder.RenameTable(
                name: "ShoppingCartProducts",
                newName: "ProductsUsersShopping");

            migrationBuilder.RenameTable(
                name: "FavoriteProducts",
                newName: "ProductsUserFavorites");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartProducts_UserId",
                table: "ProductsUsersShopping",
                newName: "IX_ProductsUsersShopping_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartProducts_ProductId",
                table: "ProductsUsersShopping",
                newName: "IX_ProductsUsersShopping_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "ProductsUserFavorites",
                newName: "IX_ProductsUserFavorites_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "ProductsUserFavorites",
                newName: "IX_ProductsUserFavorites_ProductId");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsUsersShopping",
                table: "ProductsUsersShopping",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsUserFavorites",
                table: "ProductsUserFavorites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUserFavorites_AspNetUsers_UserId",
                table: "ProductsUserFavorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUserFavorites_Products_ProductId",
                table: "ProductsUserFavorites",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUsersShopping_AspNetUsers_UserId",
                table: "ProductsUsersShopping",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUsersShopping_Products_ProductId",
                table: "ProductsUsersShopping",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
