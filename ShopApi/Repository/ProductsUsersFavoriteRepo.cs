using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class ProductsUsersFavoriteRepo
    {
        private readonly DataContext _dataContext;
        private readonly UserRepo _userRepo;

        public ProductsUsersFavoriteRepo(DataContext dataContext, UserRepo userRepo)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
        }

        public async Task AddProductToFavorite(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();

            var product = await _dataContext.Products.Include(x => x.Brand)
                .Include(x => x.ProductsUsersShopping)
                .SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));

            var productAdded = await _dataContext.ProductsUserFavorites
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                x.Product.Barcode.Equals(barcode) && x.User.Email.Equals(user.Email));

            if (productAdded == null)
            {
                await _dataContext.ProductsUserFavorites.AddAsync(new ProductsUserFavorite { Product = product, User = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<ProductsUserFavorite> DeleteProduct(string barcode)
        {

            var user = await _userRepo.GetCurrentUser();
            var productToRemove = await _dataContext.ProductsUserFavorites
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Email.Equals(user.Email));

            if (productToRemove != null)
            {
                _dataContext.ProductsUserFavorites.Remove(productToRemove);
                await _dataContext.SaveChangesAsync();
            }

            return productToRemove;

        }

        public async Task<List<ProductsUserFavorite>> GetProductsFavorite()
        {
            var user = await _userRepo.GetCurrentUser();

            return await _dataContext.ProductsUserFavorites.Include(x => x.Product)
                                                            .Where(x => x.User.Email.Equals(user.Email))
                                                            .ToListAsync();
        }
    }
}
