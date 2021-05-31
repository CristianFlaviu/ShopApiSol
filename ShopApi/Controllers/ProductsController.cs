using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Dto;
using ShopApi.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductService _productService;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly OrderService _orderService;
        private readonly FavoriteProductsService _favoriteProductsService;


        public ProductsController(ProductService productService, ShoppingCartService shoppingCartService, OrderService orderService, FavoriteProductsService favoriteProductsService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _favoriteProductsService = favoriteProductsService;
        }

        #region  Products

        [HttpGet("get-products")]
        public async Task<CommandResult<List<ProductsGeneralDto>>> GetProducts()
            => CommandResult<List<ProductsGeneralDto>>.Success(await _productService.GetAll());
        [HttpGet("get-product-by-barcode/{barcode}")]
        public async Task<CommandResult<Product>> GetProductByBarcode([FromRoute] string barcode)
            => CommandResult<Product>.Success(await _productService.GetByBarcode(barcode));

        [HttpGet("get-products-category/{category}")]
        public async Task<CommandResult<List<Product>>> GetProductsFromCategory([FromRoute] string category)
            => CommandResult<List<Product>>.Success(await _productService.GetAllFromCategory(category));

        #endregion

        #region ShoppingCart

        [HttpGet("add-product-shopping-cart/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToShoppingCart([FromRoute] string barcode)
        {
            await _shoppingCartService.AddProduct(barcode);

            return CommandResult<bool>.Success(true);
        }

        [HttpGet("get-shopping-cart-products")]
        public async Task<CommandResult<List<ProductsUsersShoppingCart>>> GetShoppingCartProduct()
            => CommandResult<List<ProductsUsersShoppingCart>>.Success(await _shoppingCartService.GetProductsShoppingCartNotOrderedUser());

        [HttpPost("set-quantity-product-shopping-cart")]
        public async Task<CommandResult<bool>> LowerProductQuantity([FromBody] ProductSetQuantity productSetQuantity)
        {
            await _shoppingCartService.SetQuantityProductShoppingCart(productSetQuantity.Barcode, productSetQuantity.Quantity);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("delete-shopping-cart-product/{barcode}")]
        public async Task<CommandResult<bool>> DeleteProductFromShoppingCart([FromRoute] string barcode)
        {
            await _shoppingCartService.DeleteProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        #endregion

        #region  Orders

        [HttpGet("get-orders")]
        public async Task<CommandResult<List<Order>>> GetOrders()
        {
            return CommandResult<List<Order>>.Success(await _orderService.GetOrders());
        }

        [HttpPost("place-order-without-payment")]
        public async Task<CommandResult<bool>> PlaceOrder([FromBody] int amount)
        {
            await _orderService.PlaceOrderWithOutPayment();
            return CommandResult<bool>.Success(true);
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<CommandResult<Order>> GetOrderById(int id)
        {
            return CommandResult<Order>.Success(await _orderService.GetOrderById(id));
        }

        [HttpPost("pay-order")]
        public async Task<CommandResult<bool>> PayOrder([FromBody] PaymentDto paymentDto)
        {
            await _orderService.PlaceOrderWithPayment(paymentDto.CardNumber);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("pay-order-later-payment")]
        public async Task<CommandResult<bool>> PayOrderLaterPayment([FromBody] PaymentDto paymentDto)
        {
            await _orderService.PayOrderLaterPayment(paymentDto.OrderId, paymentDto.CardNumber);
            return CommandResult<bool>.Success(true);
        }

        [HttpGet("get-products-by-order/{id}")]
        public async Task<CommandResult<List<ProductsUsersShoppingCart>>> GetProductsByOrder(int id)
        {
            return CommandResult<List<ProductsUsersShoppingCart>>.Success(await _orderService.GetProductsByOrderId(id));
        }

        #endregion


        #region Favorite

        [HttpGet("get-favorite-products")]
        public async Task<CommandResult<List<ProductsUserFavorite>>> GetFavoriteProducts()
            => CommandResult<List<ProductsUserFavorite>>.Success(await _favoriteProductsService.GetFavoriteProducts());

        [HttpGet("add-product-favorite/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            await _favoriteProductsService.AddProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("delete-favorite-product/{barcode}")]
        public async Task<CommandResult<bool>> DeleteProductFromFavorite([FromRoute] string barcode)
        {
            await _favoriteProductsService.DeleteProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        #endregion




    }
}
