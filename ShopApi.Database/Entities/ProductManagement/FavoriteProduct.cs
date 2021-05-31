namespace ShopApi.Database.Entities.ProductManagement
{
    public class FavoriteProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public BaseUser User { get; set; }
    }
}
