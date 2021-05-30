using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApi.Core;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;

namespace ShopApi.Service
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

        public async Task<List<ProductsUserFavorite>> GetFavoriteProducts()
        {
            var user = await _userRepo.GetCurrentUser();
            return await _productsUsersFavoriteRepo.GetProductsFavorite(user);
        }

        public async Task AddProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            var product = await _productRepo.GetByBarcode(barcode);
            await _productsUsersFavoriteRepo.AddProductToFavorite(product, user);
        }

        public async Task DeleteProduct(string barcode)
        {
            var user = await _userRepo.GetCurrentUser();
            await _productsUsersFavoriteRepo.DeleteProduct(barcode, user);
        }
    }
}
