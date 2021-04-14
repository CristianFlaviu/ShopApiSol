using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;
        private readonly ProductUserShoppingCartRepo _productUserShoppingCartRepo;
        private readonly ProductsUsersFavoriteRepo _productsUsersFavoriteRepo;


        public ProductsController(ProductRepo productRepo, CategoryRepo categoryRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo, ProductsUsersFavoriteRepo productsUsersFavoriteRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _productUserShoppingCartRepo = productUserShoppingCartRepo;
            _productsUsersFavoriteRepo = productsUsersFavoriteRepo;
        }

        [HttpGet("get-products")]
        public async Task<CommandResult<List<Product>>> GetProducts()
            => CommandResult<List<Product>>.Success(await _productRepo.GetAll());
        [HttpGet("get-product-by-barcode/{barcode}")]
        public async Task<CommandResult<Product>> GetProductByBarcode([FromRoute] string barcode)
            => CommandResult<Product>.Success(await _productRepo.GetByBarcode(barcode));

        [HttpGet("get-products-category/{category}")]
        public async Task<CommandResult<List<Product>>> GetProductsFromCategory([FromRoute] string category)
            => CommandResult<List<Product>>.Success(await _categoryRepo.GetAllFromCategory(category));

        #region ShoppingCart

        [HttpGet("get-shopping-cart-products")]
        public async Task<CommandResult<List<ProductsUsersShoppingCart>>> GetShoppingCartProduct()
            => CommandResult<List<ProductsUsersShoppingCart>>.Success(await _productUserShoppingCartRepo.GetProductsShoppingCartUser());

        [HttpGet("add-product-shopping-cart/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToShoppingCart([FromRoute] string barcode)
        {
            await _productUserShoppingCartRepo.AddProductToShoppingCart(barcode);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("delete-shopping-cart-product/{barcode}")]
        public async Task<CommandResult<ProductsUsersShoppingCart>> DeleteProductFromShoppingCart([FromRoute] string barcode)
            => CommandResult<ProductsUsersShoppingCart>.Success(await _productUserShoppingCartRepo.DeleteProduct(barcode));

        #endregion

        #region Favorite

        [HttpGet("get-favorite-products")]
        public async Task<CommandResult<List<ProductsUserFavorite>>> GetFavoriteProducts()
            => CommandResult<List<ProductsUserFavorite>>.Success(await _productsUsersFavoriteRepo.GetProductsFavorite());

        [HttpGet("add-product-favorite/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            await _productsUsersFavoriteRepo.AddProductToFavorite(barcode);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("delete-favorite-product/{barcode}")]
        public async Task<CommandResult<ProductsUserFavorite>> DeleteProductFromFavorite([FromRoute] string barcode)
            => CommandResult<ProductsUserFavorite>.Success(await _productsUsersFavoriteRepo.DeleteProduct(barcode));

        #endregion




    }
}
