using System;
using System.Threading.Tasks;
using ShopApi.Database.Data;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;

namespace ShopApi.Repository
{
    public class PaymentRepo
    {
        private readonly DataContext _dataContext;

        public PaymentRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddPayment(double amount, string cardNumber, Order order, BaseUser user)
        {
            var p = new Payment { Amount = amount, Date = DateTime.Now, User = user, CardNumber = cardNumber, Order = order };
            await _dataContext.Payments.AddAsync(p);
            await _dataContext.SaveChangesAsync();
        }
    }
}
