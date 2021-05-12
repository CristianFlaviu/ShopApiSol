

using System.Text.Json.Serialization;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class ProductsUsersShoppingCart
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public BaseUser User { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public bool IsOrdered { get; set; }
    }
}
