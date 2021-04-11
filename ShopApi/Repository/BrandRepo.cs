using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class BrandRepo
    {
        private readonly DataContext _dataContext;

        public BrandRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task SaveAsync(Brand brand)
        {
            await _dataContext.Brands.AddAsync(brand);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<Brand> GetByName(string name)
        {
            return await _dataContext.Brands.SingleOrDefaultAsync(x => x.Name.Equals(name));
        }
    }
}
