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

        public OrderRepo(DataContext dataContext, UserRepo userRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
            _productUserShoppingCartRepo = productUserShoppingCartRepo;
        }

        public async Task PlaceOrderWithOutPayment(int amount)
        {
            var user = await _userRepo.GetCurrentUser();
            var products = await _productUserShoppingCartRepo.GetProductsShoppingCartUser();

            var order = new Order
            {
                OrderDate = new DateTime(),
                Products = products,
                Payment = null,
                User = user,
                Amount = amount
            };
            await _productUserShoppingCartRepo.DeleteAll();
            await _dataContext.SaveChangesAsync();
        }
    }
}
