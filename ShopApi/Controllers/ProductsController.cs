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
        private readonly ProductCategoryRepo _productCategoryRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProductsController(ProductRepo productRepo, ProductCategoryRepo productCategoryRepo, IHttpContextAccessor httpContextAccessor)
        {
            _productRepo = productRepo;
            _productCategoryRepo = productCategoryRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("get-products")]
        public async Task<CommandResult<List<ProductCategory>>> GetProducts()
            => CommandResult<List<ProductCategory>>.Success(await _productCategoryRepo.GetAll());

        [HttpGet("get-product-by-barcode/{barcode}")]
        public async Task<CommandResult<ProductCategory>> GetProductByBarcode([FromRoute] string barcode)
            => CommandResult<ProductCategory>.Success(await _productCategoryRepo.GetByBarcode(barcode));

        [HttpGet("get-products-by-category/{category}")]
        public async Task<CommandResult<List<ProductCategory>>> GetProductsFromCategory([FromRoute] string category)
            => CommandResult<List<ProductCategory>>.Success(await _productCategoryRepo.GetProductsFromCategory(category));

        [HttpGet("add-products-to-favorite/{barcode}")]
        public async Task<CommandResult<bool>> AddProductToFavorite([FromRoute] string barcode)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var handler = new JwtSecurityTokenHandler();
            var jwt = httpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            var claims = handler.ReadJwtToken(jwt).Claims;
            var email = claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value;

            await _productCategoryRepo.AddProductToFavorite(barcode, email);




            return CommandResult<bool>.Success(true);
        }






    }
}
