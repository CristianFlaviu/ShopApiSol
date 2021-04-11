using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public Product Product { get; set; }

        [Column(TypeName = "jsonb")]
        public string Attributes { get; set; }

    }
}
