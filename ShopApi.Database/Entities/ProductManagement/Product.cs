using System;
using System.Collections.Generic;

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
        public List<BaseUser> FavoriteUsers { get; set; }
        public List<BaseUser> ShoppingCartUsers { get; set; }


    }
}
