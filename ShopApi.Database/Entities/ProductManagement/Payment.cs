using System;
using System.Text.Json.Serialization;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string CardNumber { get; set; }
        public BaseUser User { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
    }
}
