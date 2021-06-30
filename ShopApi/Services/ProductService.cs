using Polly;
using Polly.Retry;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;

        private readonly AsyncRetryPolicy _asyncRetryPolicy;

        public ProductService(ProductRepo productRepo)
        {
            _productRepo = productRepo;
            _asyncRetryPolicy = Policy
                              .Handle<Exception>()
                             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); ;
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
            return await _productRepo.GetAllFromCategory(category);
        }

    }
}

//var res = await _asyncRetryPolicy.ExecuteAndCaptureAsync(async () =>
//  {
//      var products = await _productRepo.GetAll();
//      return _mapper.Map<List<ProductsSearch>>(products);
//  });

//if(res.Outcome == OutcomeType.Failure)