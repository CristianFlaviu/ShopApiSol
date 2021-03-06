﻿namespace ShopApi.Database.Entities.ProductManagement
{
    public class ShoppingCartProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public BaseUser User { get; set; }
        public int Quantity { get; set; }
    }
}
