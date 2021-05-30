using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Database.Entities;

namespace ShopApi.Repository
{
    public class ProductUserShoppingCartRepo
    {
        private readonly DataContext _dataContext;

        public ProductUserShoppingCartRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddProductToShoppingCart(Product product, BaseUser user)
        {
            var productAdded = await _dataContext.ProductsUsersShopping
                .SingleOrDefaultAsync(x =>
                x.Product.Barcode.Equals(product.Barcode) && x.User.Id.Equals(user.Id) && x.IsOrdered == false);

            if (productAdded == null)
            {
                await _dataContext.ProductsUsersShopping.AddAsync(new ProductsUsersShoppingCart { Product = product, User = user, Quantity = 1, IsOrdered = false });
            }
            else
            {
                productAdded.Quantity++;
            }
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsShoppingCartNotOrderedUser(string userId)
        {
            return await _dataContext.ProductsUsersShopping.Include(x => x.Product)
                .Where(x => x.User.Id.Equals(userId) && x.IsOrdered == false)
                .ToListAsync();
        }

        public async Task SetQuantityProductShoppingCart(string barcode, int quantity, string userId)
        {
            var productUserShoppingCart = await _dataContext.ProductsUsersShopping
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Id.Equals(userId) && !x.IsOrdered);

            if (productUserShoppingCart != null)
            {
                productUserShoppingCart.Quantity = quantity;

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(string barcode, string userId)
        {
            var productToBeDeleted = await _dataContext.ProductsUsersShopping
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Id.Equals(userId) && !x.IsOrdered);

            if (productToBeDeleted != null)
            {
                _dataContext.ProductsUsersShopping.Remove(productToBeDeleted);
                await _dataContext.SaveChangesAsync();
            }

        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsByOrderId(int id, string userId)
        {
            return await _dataContext.ProductsUsersShopping.Include(x => x.Product)
                                                            .Include(x => x.Order)
                                                            .Where(x => x.User.Id.Equals(userId) && x.Order.Id == id)
                                                            .ToListAsync();
        }

        public async Task MarkProductsAsOrdered(string userId)
        {
            await _dataContext.ProductsUsersShopping.Where(x => x.User.Id.Equals(userId) && x.IsOrdered == false)
                 .ForEachAsync(x => x.IsOrdered = true);

            await _dataContext.SaveChangesAsync();

        }

    }
}
