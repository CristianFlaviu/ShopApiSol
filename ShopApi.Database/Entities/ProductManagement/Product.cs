using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public double BasePrice { get; set; }
        public int Discount { get; set; }
        public double Score { get; set; }
        public DateTime Availability { get; set; }
        public string Barcode { get; set; }
        public int UnitsAvailable { get; set; }
        public string PathToImage { get; set; }
        public Brand Brand { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public string Attributes { get; set; }
        [JsonIgnore]
        public List<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        [JsonIgnore]
        public List<FavoriteProduct> FavoriteProducts { get; set; }

        [JsonIgnore]
        public List<OrderedProduct> OrderedProducts { get; set; }
    }
}
