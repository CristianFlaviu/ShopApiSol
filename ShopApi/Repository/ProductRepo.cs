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
        public async Task<Product> GetByNameAsync(string name)
        {
            return await _dataContext.Products.SingleOrDefaultAsync(x => x.Title.Equals(name));
        }

        public async Task<List<Product>> GetAll()
        {
            return await _dataContext.Products.Include(x => x.Brand).ToListAsync();
        }

        public async Task<Product> GetByBarcodeAsync(string barcode)
        {
            return await _dataContext.Products.Include(x => x.Brand).
                         SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));
        }

        public async Task UpdateProductAsync(Product product)
        {

            var productUpdate = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == product.Id);
            productUpdate.Barcode = product.Barcode;
            await _dataContext.SaveChangesAsync();

        }

    }
}
