using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;

        public ProductsController(ProductRepo productRepo, CategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        [HttpGet("get-products")]
        public async Task<CommandResult<List<Product>>> GetProducts()
            => CommandResult<List<Product>>.Success(await _productRepo.GetAll());
        [HttpGet("get-product-by-barcode/{barcode}")]
        public async Task<CommandResult<Product>> GetProductByBarcode([FromRoute] string barcode)
            => CommandResult<Product>.Success(await _productRepo.GetByBarcode(barcode));

        [HttpGet("get-products-by-category/{category}")]
        public async Task<CommandResult<List<Product>>> GetProductsFromCategory([FromRoute] string category)
            => CommandResult<List<Product>>.Success(await _categoryRepo.GetAllFromCategory(category));

        [HttpGet("add-products-to-favorite/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            await _productRepo.AddProductToFavorites(barcode);

            return CommandResult<bool>.Success(true);
        }






    }
}
