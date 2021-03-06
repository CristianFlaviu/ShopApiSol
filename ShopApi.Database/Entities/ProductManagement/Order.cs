﻿using System;
using System.Collections.Generic;

namespace ShopApi.Database.Entities.ProductManagement
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }
        public DateTime OrderDate { get; set; }
        public BaseUser User { get; set; }
        public List<Payment> Payments { get; set; }
        public DateTime LimitDate { get; set; }
        public double InvoiceAmount { get; set; }

    }
}
