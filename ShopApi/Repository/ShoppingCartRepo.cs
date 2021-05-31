using System;
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
    public class ShoppingCartRepo
    {
        private readonly DataContext _dataContext;

        public ShoppingCartRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddProductToShoppingCart(Product product, BaseUser user)
        {
            var productAdded = await _dataContext.ShoppingCartProducts
                .SingleOrDefaultAsync(x =>
                x.Product.Barcode.Equals(product.Barcode) && x.User.Id.Equals(user.Id));

            if (productAdded == null)
            {
                await _dataContext.ShoppingCartProducts.AddAsync(new ShoppingCartProduct { Product = product, User = user, Quantity = 1 });
            }
            else
            {
                productAdded.Quantity++;
            }
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ShoppingCartProduct>> GetProductsShoppingCart(string userId)
        {
            return await _dataContext.ShoppingCartProducts.Include(x => x.Product)
                .Where(x => x.User.Id.Equals(userId))
                .ToListAsync();
        }

        public async Task SetQuantityProductShoppingCart(string barcode, int quantity, string userId)
        {
            var productUserShoppingCart = await _dataContext.ShoppingCartProducts
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Id.Equals(userId));

            if (productUserShoppingCart != null)
            {
                productUserShoppingCart.Quantity = quantity;

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(string barcode, string userId)
        {
            var productToBeDeleted = await _dataContext.ShoppingCartProducts
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Id.Equals(userId));

            if (productToBeDeleted != null)
            {
                _dataContext.ShoppingCartProducts.Remove(productToBeDeleted);
                await _dataContext.SaveChangesAsync();
            }

        }

        public async Task<List<ShoppingCartProduct>> GetProductsByOrderId(int id, string userId)
        {
            return await _dataContext.ShoppingCartProducts.Include(x => x.Product).ToListAsync();

        }

        public async Task RemoveUserProducts(string userId)
        {
            var useProducts = await _dataContext.ShoppingCartProducts.Include(
                x => x.User).Where(x => x.User.Id.Equals(userId)).ToListAsync();

            _dataContext.ShoppingCartProducts.RemoveRange(useProducts);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ShoppingCartProduct>> GetProductsAboutToExpire()
        {
            return await _dataContext.ShoppingCartProducts
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(x => x.Product.Availability < DateTime.Now.AddDays(2)).ToListAsync();
        }

    }
}
