using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        public BaseUser User { get; set; }
        public int Quantity { get; set; }
    }
}
