namespace ShopApi.Database.Entities.ProductManagement
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double PricePerProduct { get; set; }
    }
}
