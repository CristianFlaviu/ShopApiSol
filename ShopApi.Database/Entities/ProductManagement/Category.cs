using System.Collections.Generic;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category ParentCategory { get; set; }
        public List<Product> Products { get; set; }
    }
}
