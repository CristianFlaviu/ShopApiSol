using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShopApi.Database.Migrations
{
    public partial class m5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUsersShopping_Orders_OrderId",
                table: "ProductsUsersShopping");

            migrationBuilder.DropIndex(
                name: "IX_ProductsUsersShopping_OrderId",
                table: "ProductsUsersShopping");

            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "ProductsUsersShopping");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductsUsersShopping");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NewPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "OrderedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PricePerProduct = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountPercent = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasePrice = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_OrderId",
                table: "OrderedProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_ProductId",
                table: "OrderedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_ProductId",
                table: "ProductDiscounts",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedProducts");

            migrationBuilder.DropTable(
                name: "ProductDiscounts");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "ProductsUsersShopping",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductsUsersShopping",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "NewPrice",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OldPrice",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsUsersShopping_OrderId",
                table: "ProductsUsersShopping",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUsersShopping_Orders_OrderId",
                table: "ProductsUsersShopping",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
