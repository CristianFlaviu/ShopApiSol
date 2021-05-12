using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class OrderRepo
    {
        private readonly DataContext _dataContext;
        private readonly UserRepo _userRepo;
        private readonly ProductUserShoppingCartRepo _productUserShoppingCartRepo;
        private readonly PaymentRepo _paymentRepo;

        public OrderRepo(DataContext dataContext, UserRepo userRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo, PaymentRepo paymentRepo)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
            _productUserShoppingCartRepo = productUserShoppingCartRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task PlaceOrderWithOutPayment(int amount)
        {
            var user = await _userRepo.GetCurrentUser();
            var products = await _productUserShoppingCartRepo.GetProductsShoppingCartNotOrderedUser();
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Products = products,
                User = user,
                Amount = amount,
            };

            await _dataContext.Orders.AddAsync(order);
            await _productUserShoppingCartRepo.MarkProductsAsOrdered();
            await _dataContext.SaveChangesAsync();
        }

        public async Task PlaceOrderWithPayment(int amount, string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var products = await _productUserShoppingCartRepo.GetProductsShoppingCartNotOrderedUser();
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Products = products,
                User = user,
                Amount = amount,
            };
            await _paymentRepo.SaveAsync(amount, cardNumber, order);
            await _productUserShoppingCartRepo.MarkProductsAsOrdered();
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            var user = await _userRepo.GetCurrentUser();

            return await _dataContext.Orders.Include(x => x.Products)
                                                            .Include(x => x.Payments)
                                                           .Where(x => x.User.Email.Equals(user.Email))
                                                           .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _dataContext.Orders.Include(x => x.Products)
                .Include(x => x.Payments)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
