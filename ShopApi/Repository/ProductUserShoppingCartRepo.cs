using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class ProductUserShoppingCartRepo
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccess;
        private readonly UserRepo _userRepo;

        public ProductUserShoppingCartRepo(DataContext dataContext, IHttpContextAccessor httpContextAccess, UserRepo userRepo)
        {
            _dataContext = dataContext;
            _httpContextAccess = httpContextAccess;
            _userRepo = userRepo;
        }

        public async Task AddProductToShoppingCart(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();

            var product = await _dataContext.Products.Include(x => x.Brand)
                .Include(x => x.ProductsUsersShopping)
                .SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));

            var productAdded = await _dataContext.ProductsUsersShopping
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                x.Product.Barcode.Equals(barcode) && x.User.Email.Equals(user.Email) && x.IsOrdered == false);

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

        public async Task SetQuantityProductShoppingCart(string barcode, int quantity)
        {
            var user = await _userRepo.GetCurrentUser();

            var productUserShoppingCart = await _dataContext.ProductsUsersShopping
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Email.Equals(user.Email));

            if (productUserShoppingCart != null)
            {
                productUserShoppingCart.Quantity = quantity;

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<ProductsUsersShoppingCart> DeleteProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            var productAdded = await _dataContext.ProductsUsersShopping
                .Include(x => x.Product)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x =>
                    x.Product.Barcode.Equals(barcode) && x.User.Email.Equals(user.Email) && x.IsOrdered==false);

            if (productAdded != null)
            {
                _dataContext.ProductsUsersShopping.Remove(productAdded);
                await _dataContext.SaveChangesAsync();
            }

            return productAdded;

        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsShoppingCartNotOrderedUser()
        {
            var user = await _userRepo.GetCurrentUser();

            return await _dataContext.ProductsUsersShopping.Include(x => x.Product)
                                                            .Where(x => x.User.Email.Equals(user.Email) && x.IsOrdered == false)
                                                            .ToListAsync();
        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsShoppingCartOrderedUser()
        {
            var user = await _userRepo.GetCurrentUser();

            return await _dataContext.ProductsUsersShopping.Include(x => x.Product)
                                                          .Where(x => x.User.Email.Equals(user.Email) && x.IsOrdered == true)
                                                           .ToListAsync();
        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsByOrderId(int id)
        {
            var user = await _userRepo.GetCurrentUser();

            return await _dataContext.ProductsUsersShopping.Include(x => x.Product)
                                                            .Include(x => x.Order)
                                                            .Where(x => x.User.Email.Equals(user.Email) && x.Order.Id == id)
                                                            .ToListAsync();
        }


        public async Task MarkProductsAsOrdered()
        {
            var user = await _userRepo.GetCurrentUser();
            await _dataContext.ProductsUsersShopping.Where(x => x.User.Email.Equals(user.Email) && x.IsOrdered == false)
                 .ForEachAsync(x => x.IsOrdered = true);

            await _dataContext.SaveChangesAsync();

        }

        public async Task DeleteAll()
        {
            _dataContext.ProductsUsersShopping.RemoveRange(await _dataContext.ProductsUsersShopping.ToListAsync());
        }
    }
}
