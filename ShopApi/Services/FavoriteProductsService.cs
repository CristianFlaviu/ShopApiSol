using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class FavoriteProductsService
    {
        private readonly UserRepo _userRepo;
        private readonly ProductsUsersFavoriteRepo _productsUsersFavoriteRepo;
        private readonly ProductRepo _productRepo;

        public FavoriteProductsService(UserRepo userRepo, ProductsUsersFavoriteRepo productsUsersFavoriteRepo, ProductRepo productRepo)
        {
            _userRepo = userRepo;
            _productsUsersFavoriteRepo = productsUsersFavoriteRepo;
            _productRepo = productRepo;
        }

        public async Task<List<FavoriteProduct>> GetFavoriteProducts()
        {
            var user = await _userRepo.GetCurrentUser();
            return await _productsUsersFavoriteRepo.GetProductsFavorite(user);
        }

        public async Task AddProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            var product = await _productRepo.GetByBarcode(barcode);
            await _productsUsersFavoriteRepo.AddProductToFavorite(new FavoriteProduct { Product = product, User = user });
        }

        public async Task DeleteProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            await _productsUsersFavoriteRepo.DeleteProduct(barcode, user);
        }
    }
}
