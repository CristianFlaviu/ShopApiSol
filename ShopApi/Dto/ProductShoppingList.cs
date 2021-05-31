namespace ShopApi.Dto
{
    public class ProductShoppingList
    {
        public string PathToImage { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public double NewPrice { get; set; }
        public int Discount { get; set; }
        public double OldPrice { get; set; }
        public int Quantity { get; set; }
    }
}
