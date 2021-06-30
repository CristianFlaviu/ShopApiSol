using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Threading.Tasks;

namespace ShopApi.Repositories
{
    public class PaymentRepo
    {
        private readonly DataContext _dataContext;

        public PaymentRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddPayment(Payment payment)
        {
            await _dataContext.Payments.AddAsync(payment);
            await _dataContext.SaveChangesAsync();
        }
    }
}
