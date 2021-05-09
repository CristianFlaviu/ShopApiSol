using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities.ProductManagement;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class CategoryRepo
    {
        private readonly DataContext _dataContext;

        public CategoryRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task SaveAsync(Category category)
        {
            await _dataContext.Category.AddAsync(category);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Category> GetByName(string name)
        {
            return await _dataContext.Category.SingleOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Product>> GetAllFromCategory(string category)
        {
            return (await _dataContext.Category.Include(x => x.Products)
                                               .SingleOrDefaultAsync(x => x.Name.Equals(category)))?.Products;
        }
    }
}
