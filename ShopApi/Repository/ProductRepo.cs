using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopApi.Database.Entities;

namespace ShopApi.Repository
{
    public class ProductRepo
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccess;


        public ProductRepo(DataContext dataContext, IHttpContextAccessor httpContextAccess)
        {
            _dataContext = dataContext;
            _httpContextAccess = httpContextAccess;
        }

        public async Task SaveAsync(Product product)
        {
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            return await _dataContext.Products.Include(x => x.Brand)
                                              .Include(x => x.FavoriteUsers)
                                              .Include(x => x.ShoppingCartUsers)
                                              .AsSplitQuery()
                                              .ToListAsync();
        }

        public async Task<Product> GetByBarcode(string barcode)
        {
            return await _dataContext.Products.Include(x => x.Brand)
                .Include(x => x.FavoriteUsers)
                .Include(x => x.ShoppingCartUsers)
                .SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));
        }

        public async Task AddProductToFavorites(string barcode)
        {
            var httpContext = _httpContextAccess.HttpContext;
            var handler = new JwtSecurityTokenHandler();
            var jwt = httpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            var claims = handler.ReadJwtToken(jwt).Claims;
            var email = claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value;

            var user = (BaseUser)await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            var product = await GetByBarcode(barcode);

            product.FavoriteUsers.Add(user);




            await _dataContext.SaveChangesAsync();
        }
    }
}
