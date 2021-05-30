using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Dto;
using ShopApi.Repository;

namespace ShopApi.Service
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;

        public ProductService(ProductRepo productRepo, CategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productRepo.GetAll();
        }

        public async Task<Product> GetByBarcode(string barcode)
        {

            return await _productRepo.GetByBarcode(barcode);

        }

        public async Task<List<Product>> GetAllFromCategory(string category)
        {
            return await _categoryRepo.GetAllFromCategory(category);
        }


    }
}
