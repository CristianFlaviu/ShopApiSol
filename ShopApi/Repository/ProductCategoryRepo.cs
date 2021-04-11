using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using ShopApi.Database.Entities;

namespace ShopApi.Repository
{
    public class ProductCategoryRepo
    {
        private readonly DataContext _dataContext;

        public ProductCategoryRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SaveAsync(ProductCategory productCategory)
        {
            await _dataContext.ProductCategory.AddAsync(productCategory);
            await _dataContext.SaveChangesAsync();

        }

        public async Task<ProductCategory> GetByName(string name)
        {
            return await _dataContext.ProductCategory.SingleOrDefaultAsync(x => x.Product.Title.Equals(name));
        }

        public async Task AddProductToFavorite(string barcode, string email)
        {
            var productCategory = await GetByBarcode(barcode);
            var user = (BaseUser)await _dataContext.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));

            productCategory.Product.FavoriteUsers.Add(user);

            await _dataContext.SaveChangesAsync();

        }


        public async Task<ProductCategory> GetByBarcode(string barcode)
        {
            return await _dataContext.ProductCategory.Include(x => x.Category)
                .Include(x => x.Product)
                .Include(x => x.Product.FavoriteUsers)
                .SingleOrDefaultAsync(x => x.Product.Barcode.Equals(barcode));
        }

        public async Task<List<ProductCategory>> GetProductsFromCategory(string categoryName)
        {
            return await _dataContext.ProductCategory.Include(x => x.Category)
                .Include(x => x.Product)
                .Include(x => x.Product.FavoriteUsers)
                .Where(x => x.Category.Name.Equals(categoryName))
                .ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await _dataContext.ProductCategory.Include(x => x.Category)
                                                     .Include(x => x.Product)
                                                     .Include(x => x.Product.FavoriteUsers)
                                                     .ToListAsync();
        }
    }
}
