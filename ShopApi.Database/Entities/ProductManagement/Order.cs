using System;
using System.Collections.Generic;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Order
    {
        public int Id { get; set; }
        public List<ProductsUsersShoppingCart> Products { get; set; }
        public DateTime OrderDate { get; set; }
        public BaseUser User { get; set; }
        public double Amount { get; set; }
        public List<Payment> Payments { get; set; }

    }
}
