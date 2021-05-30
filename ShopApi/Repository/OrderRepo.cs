using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Database.Entities;

namespace ShopApi.Repository
{
    public class OrderRepo
    {
        private readonly DataContext _dataContext;

        public OrderRepo(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task<Order> PlaceOrder(List<ProductsUsersShoppingCart> products, BaseUser user, double amount)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Products = products,
                User = user,
                Amount = amount
            };
            await _dataContext.Orders.AddAsync(order);
            await _dataContext.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersCurrentUser(string userId)
        {
            return await _dataContext.Orders.Include(x => x.Products)
                                                           .Include(x => x.Payments)
                                                           .Where(x => x.User.Id.Equals(userId))
                                                           .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId, string userId)
        {
            return await _dataContext.Orders.Include(x => x.Products)
                .Include(x => x.User)
                .Include(x => x.Payments)
                .SingleOrDefaultAsync(x => x.Id == orderId && x.User.Id.Equals(userId));
        }
    }
}
