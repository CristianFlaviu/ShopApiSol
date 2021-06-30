using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class ShoppingCartService
    {
        private readonly UserRepo _userRepo;
        private readonly ProductRepo _productRepo;
        private readonly ShoppingCartRepo _productUserShoppingCart;

        public ShoppingCartService(UserRepo userRepo, ProductRepo productRepo, ShoppingCartRepo productUserShoppingCart)
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
            _productUserShoppingCart = productUserShoppingCart;
        }

        public async Task AddProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            var product = await _productRepo.GetByBarcode(barcode);
            await _productUserShoppingCart.AddProductToShoppingCart(product, user);
        }

        public async Task<List<ShoppingCartProduct>> GetProductsShoppingCart()
        {
            var user = await _userRepo.GetCurrentUser();
            return await _productUserShoppingCart.GetProductsShoppingCart(user.Id);
        }

        public async Task SetQuantityProductShoppingCart(string barcode, int quantity)
        {
            var user = await _userRepo.GetCurrentUser();
            await _productUserShoppingCart.SetQuantityProductShoppingCart(barcode, quantity, user.Id);
        }

        public async Task DeleteProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            await _productUserShoppingCart.DeleteProduct(barcode, user.Id);
        }

    }
}
