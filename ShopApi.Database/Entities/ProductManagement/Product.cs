using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopApi.Database.Entities.ProductManagement

{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Score { get; set; }
        public double NewPrice { get; set; }
        public double OldPrice { get; set; }
        public int Discount { get; set; }
        public DateTime Availability { get; set; }
        public string Barcode { get; set; }
        public int UnitsAvailable { get; set; }
        public string PathToImage { get; set; }
        public Brand Brand { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }

        public string Attributes { get; set; }
        [JsonIgnore]
        public List<ProductsUsersShoppingCart> ProductsUsersShopping { get; set; }
        [JsonIgnore]
        public List<ProductsUserFavorite> ProductUserFavorites { get; set; }



    }
}
