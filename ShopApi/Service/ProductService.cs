using System;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Polly;
using ShopApi.Dto;

namespace ShopApi.Service
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductService(ProductRepo productRepo, CategoryRepo categoryRepo, IMapper mapper, ILogger logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductsGeneralDto>> GetAll()
        {
            //var policy = Polly.Policy<List<ProductsGeneralDto>>.Handle<Exception>()
            //    .RetryAsync(3, retryAttemp => 200);
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return await policy.ExecuteAsync(async () =>
              {
                  var products = await _productRepo.GetAll();
                  return _mapper.Map<List<ProductsGeneralDto>>(products);
              });

            var res = await policy.ExecuteAndCaptureAsync(async () =>
              {
                  var products = await _productRepo.GetAll();
                  return _mapper.Map<List<ProductsGeneralDto>>(products);
              });

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
