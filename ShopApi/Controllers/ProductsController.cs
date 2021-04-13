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

        public ProductsController(ProductRepo productRepo, CategoryRepo categoryRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _productUserShoppingCartRepo = productUserShoppingCartRepo;
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

        [HttpGet("add-product-shopping-cart/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            await _productUserShoppingCartRepo.AddProductToShoppingCart(barcode);

            return CommandResult<bool>.Success(true);
        }
        [HttpGet("get-shopping-cart-products")]
        public async Task<CommandResult<List<ProductsUsersShoppingCart>>> GetShoppingCartProduct()
            => CommandResult<List<ProductsUsersShoppingCart>>.Success(await _productUserShoppingCartRepo.GetProductsShoppingCartUser());

        [HttpPost("delete-shopping-cart-product/{barcode}")]
        public async Task<CommandResult<ProductsUsersShoppingCart>> DeleteProductFromShoppingCart([FromRoute] string barcode)
            => CommandResult<ProductsUsersShoppingCart>.Success(await _productUserShoppingCartRepo.DeleteProduct(barcode));
    }
}
