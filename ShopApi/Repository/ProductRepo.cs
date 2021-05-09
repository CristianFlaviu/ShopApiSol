using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class ProductRepo
    {
        private readonly DataContext _dataContext;

        public ProductRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SaveAsync(Product product)
        {
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            return await _dataContext.Products.Include(x => x.Brand)
                                              .Include(x => x.ProductsUsersShopping)
                                              .AsSplitQuery()
                                              .ToListAsync();
        }

        public async Task<Product> GetByBarcode(string barcode)
        {
            return await _dataContext.Products.Include(x => x.Brand)
                .Include(x => x.ProductsUsersShopping)
                .SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));
        }


    }
}
