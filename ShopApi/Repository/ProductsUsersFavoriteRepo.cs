using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class ProductsUsersFavoriteRepo
    {
        private readonly DataContext _dataContext;
        public ProductsUsersFavoriteRepo(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task AddProductToFavorite(Product product, BaseUser user)
        {
            var productToBeAdded = await _dataContext.FavoriteProducts
                    .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(product.Barcode) && x.User.Id.Equals(user.Id));

            if (productToBeAdded == null)
            {
                await _dataContext.FavoriteProducts.AddAsync(new FavoriteProduct { Product = product, User = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(string barcode, BaseUser user)
        {
            var productToRemove = await _dataContext.FavoriteProducts
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Id.Equals(user.Id));

            if (productToRemove != null)
            {
                _dataContext.FavoriteProducts.Remove(productToRemove);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<List<FavoriteProduct>> GetProductsFavorite(BaseUser user)
        {
            return await _dataContext.FavoriteProducts.
                Include(x => x.Product)
                .Where(x => x.User.Id.Equals(user.Id))
                .ToListAsync();
        }
    }
}
