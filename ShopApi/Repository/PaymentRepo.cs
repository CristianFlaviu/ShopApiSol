using System;
using System.Threading.Tasks;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;

namespace ShopApi.Repository
{
    public class PaymentRepo
    {
        private readonly DataContext _dataContext;
        private readonly UserRepo _userRepo;

        public PaymentRepo(DataContext dataContext, UserRepo userRepo)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
        }
        public async Task SaveAsync(int amount, string cardNumber, Order order)
        {
            var user = await _userRepo.GetCurrentUser();
            var p = new Payment { Amount = amount, Date = DateTime.Now, User = user, CardNumber = cardNumber, Order = order };
            await _dataContext.Payments.AddAsync(p);
            await _dataContext.SaveChangesAsync();
        }
    }
}
