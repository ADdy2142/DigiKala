using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Models.Orders
{
    public class OrderItem
    {
        public long Id { get; set; }
        public Order Order { get; set; }
        public ProductItem ProductItem { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
