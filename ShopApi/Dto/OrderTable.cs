using ShopApi.Database.Entities.ProductManagement;
using System;

namespace ShopApi.Dto
{
    public class OrderTable
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Payment Payment { get; set; }
        public DateTime DueDate { get; set; }
        public double ProductsCost { get; set; }
        public double Interest { get; set; }
    }
}
