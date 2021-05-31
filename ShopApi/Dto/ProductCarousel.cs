namespace ShopApi.Dto
{
    public class ProductCarousel
    {
        public string PathToImage { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public double Score { get; set; }
        public double NewPrice { get; set; }
        public int Discount { get; set; }
        public double OldPrice { get; set; }
    }
}
