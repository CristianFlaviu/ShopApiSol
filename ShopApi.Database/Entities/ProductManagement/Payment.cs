using System;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public BaseUser User { get; set; }
    }
}
