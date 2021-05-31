using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Repository
{
    public class OrderedProductRepo
    {
        private readonly DataContext _dataContext;

        public OrderedProductRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task OrderProducts(List<OrderedProduct> list)
        {
            await _dataContext.OrderedProducts.AddRangeAsync(list);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<OrderedProduct>> GetProductsByOrderId(int orderId)
        {

            return await _dataContext.OrderedProducts
                                        .Include(x => x.Product)
                                        .Include(x => x.Order)
                                        .Where(x => x.Order.Id == orderId).ToListAsync();
        }

    }
}
