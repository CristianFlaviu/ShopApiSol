using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class ProductsUserFavorite
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public BaseUser User { get; set; }
    }
}
