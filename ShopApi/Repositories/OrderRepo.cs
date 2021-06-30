using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repositories
{
    public class OrderRepo
    {
        private readonly DataContext _dataContext;

        public OrderRepo(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task<Order> PlaceOrder(Order order)
        {
            await _dataContext.Orders.AddAsync(order);
            await _dataContext.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersCurrentUser(string userId)
        {
            return await _dataContext.Orders
                                                           .Include(x => x.Payment)
                                                           .Include(x=>x.OrderedProducts)
                                                           .Where(x => x.User.Id.Equals(userId))
                                                           .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId, string userId)
        {
            return await _dataContext.Orders
                .Include(x => x.User)
                .Include(x => x.Payment)
                .Include(x=>x.OrderedProducts)
                .SingleOrDefaultAsync(x => x.Id == orderId && x.User.Id.Equals(userId));
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _dataContext.Orders
                .Include(x => x.User)
                .Include(x => x.Payment)
                .Include(x => x.OrderedProducts)
                .ToListAsync();

        }
    }
}
