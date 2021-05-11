using System;
using System.Threading.Tasks;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;

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
            var products = await _productUserShoppingCartRepo.GetProductsShoppingCartUser();
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Products = products,
                User = user,
                Amount = amount,
            };

            await _dataContext.Orders.AddAsync(order);
            await _productUserShoppingCartRepo.DeleteAll();
            await _dataContext.SaveChangesAsync();
        }

        public async Task PlaceOrderWithPayment(int amount, string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var products = await _productUserShoppingCartRepo.GetProductsShoppingCartUser();
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Products = products,
                User = user,
                Amount = amount,
            };
            await _paymentRepo.SaveAsync(amount, cardNumber, order);
            await _productUserShoppingCartRepo.DeleteAll();
            await _dataContext.SaveChangesAsync();
        }


    }
}
