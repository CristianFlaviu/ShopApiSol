using ShopApi.Database.Entities.ProductManagement;
using System;
using System.Collections.Generic;

namespace ShopApi.Dto
{
    public class OrderTable
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Payment> Payments { get; set; }
        public DateTime LimitDate { get; set; }
        public double InvoiceAmount { get; set; }
    }
}
