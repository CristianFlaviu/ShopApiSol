namespace ShopApi.Dto
{
    public class ProductDetails
    {
        public string Title { get; set; }
        public double NewPrice { get; set; }
        public double OldPrice { get; set; }
        public double Score { get; set; }
        public string Barcode { get; set; }
        public int UnitsAvailable { get; set; }
        public string PathToImage { get; set; }
        public string Brand { get; set; }
        public string Attributes { get; set; }
        public string Category { get; set; }
    }
}
