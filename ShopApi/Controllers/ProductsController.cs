using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApi.Services;

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
        private readonly IMapper _mapper;

        public ProductsController(ProductService productService, ShoppingCartService shoppingCartService, OrderService orderService, FavoriteProductsService favoriteProductsService, IMapper mapper)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _favoriteProductsService = favoriteProductsService;
            _mapper = mapper;
        }

        #region  Products

        [HttpGet("get-products")]
        public async Task<CommandResult<List<ProductSearch>>> GetProducts()
        {
            var products = await _productService.GetAll();
            return CommandResult<List<ProductSearch>>.Success(_mapper.Map<List<ProductSearch>>(products));
        }

        [HttpGet("get-product-by-barcode/{barcode}")]
        public async Task<CommandResult<ProductDetails>> GetProductByBarcode([FromRoute] string barcode)
        {
            var product = await _productService.GetByBarcode(barcode);
            return CommandResult<ProductDetails>.Success(_mapper.Map<ProductDetails>(product));
        }

        [HttpGet("get-products-category/{category}")]
        public async Task<CommandResult<List<ProductCarousel>>> GetProductsFromCategory([FromRoute] string category)
        {
            var products = await _productService.GetAllFromCategory(category);
            return CommandResult<List<ProductCarousel>>.Success(_mapper.Map<List<ProductCarousel>>(products));
        }

        [HttpGet("get-products-by-order/{id}")]
        public async Task<CommandResult<List<ProductShoppingList>>> GetProductsByOrder(int id)
        {
            var products = await _orderService.GetProductsByOrderId(id);

            return CommandResult<List<ProductShoppingList>>.Success(_mapper.Map<List<ProductShoppingList>>(products));
        }


        #endregion

        #region ShoppingCart

        [HttpGet("add-product-shopping-cart/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToShoppingCart([FromRoute] string barcode)
        {
            await _shoppingCartService.AddProduct(barcode);

            return CommandResult<bool>.Success(true);
        }

        [HttpGet("get-shopping-cart-products")]
        public async Task<CommandResult<List<ProductShoppingList>>> GetShoppingCartProduct()
        {
            var products = await _shoppingCartService.GetProductsShoppingCart();
            return CommandResult<List<ProductShoppingList>>.Success(_mapper.Map<List<ProductShoppingList>>(products));

        }

        [HttpPost("set-quantity-product-shopping-cart")]
        public async Task<CommandResult<bool>> SetProductQuantity([FromBody] ProductSetQuantity productSetQuantity)
        {
            await _shoppingCartService.SetQuantityProductShoppingCart(productSetQuantity.Barcode, productSetQuantity.Quantity);
            return CommandResult<bool>.Success(true);
        }

        [HttpDelete("delete-shopping-cart-product/{barcode}")]
        public async Task<CommandResult<bool>> DeleteProductFromShoppingCart([FromRoute] string barcode)
        {
            await _shoppingCartService.DeleteProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        #endregion

        #region Favorite

        [HttpGet("get-favorite-products")]
        public async Task<CommandResult<List<ProductShoppingList>>> GetFavoriteProducts()
        {
            var products = await _favoriteProductsService.GetFavoriteProducts();
            return CommandResult<List<ProductShoppingList>>.Success(_mapper.Map<List<ProductShoppingList>>(products));
        }


        [HttpGet("add-product-favorite/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            await _favoriteProductsService.AddProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        [HttpDelete("delete-favorite-product/{barcode}")]
        public async Task<CommandResult<bool>> DeleteProductFromFavorite([FromRoute] string barcode)
        {
            await _favoriteProductsService.DeleteProduct(barcode);
            return CommandResult<bool>.Success(true);
        }

        #endregion




    }
}
