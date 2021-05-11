using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Dto;
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
        private readonly OrderRepo _orderRepo;
        private readonly PaymentRepo _paymentRepo;


        public ProductsController(ProductRepo productRepo, CategoryRepo categoryRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo, ProductsUsersFavoriteRepo productsUsersFavoriteRepo, OrderRepo orderRepo, PaymentRepo paymentRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _productUserShoppingCartRepo = productUserShoppingCartRepo;
            _productsUsersFavoriteRepo = productsUsersFavoriteRepo;
            _orderRepo = orderRepo;
            _paymentRepo = paymentRepo;
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

        [HttpPost("set-quantity-product-shopping-cart")]
        public async Task<CommandResult<bool>> LowerProductQuantity([FromBody] ProductSetQuantity productSetQuantity)
        {
            await _productUserShoppingCartRepo.SetQuantityProductShoppingCart(productSetQuantity.Barcode, productSetQuantity.Quantity);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("place-order-without-payment")]
        public async Task<CommandResult<bool>> PlaceOrder([FromBody] int amount)
        {
            await _orderRepo.PlaceOrderWithOutPayment(amount);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("pay-order")]
        public async Task<CommandResult<bool>> PayOrder([FromBody] PaymentDto paymentDto)
        {
            await _orderRepo.PlaceOrderWithPayment(paymentDto.Amount, paymentDto.CardNumber);
            return CommandResult<bool>.Success(true);
        }
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
