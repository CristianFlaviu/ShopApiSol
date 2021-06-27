using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<List<Product>> GetAll()
        {
            return await _dataContext.Products.Include(x => x.Brand)
                                              .ToListAsync();
        }

        public async Task<Product> GetByBarcode(string barcode)
        {
            return await _dataContext.Products.Include(x => x.Brand)
                                             .Include(x => x.Category)
                                             .SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));
        }
        public async Task<List<Product>> GetAllFromCategory(string category)
        {
            return await _dataContext.Products.Include(x => x.Category)
                .Where(x => x.Category.Name.Equals(category))
                .ToListAsync();

        }

        public async Task ReduceProductQuantity(string barcode, int quantity)
        {
            var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Barcode.Equals(barcode));
            product.UnitsAvailable -= quantity;
            await _dataContext.SaveChangesAsync();

        }

    }
}
