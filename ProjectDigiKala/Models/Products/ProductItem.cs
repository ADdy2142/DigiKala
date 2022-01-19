using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Tags;

namespace ProjectDigiKala.Models.Products
{
    public class ProductItem
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public byte Quantity { get; set; }
        public Product Product { get; set; }
        public IEnumerable<ProductItemTagValue> ProductItemTagValues { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
    }
}
