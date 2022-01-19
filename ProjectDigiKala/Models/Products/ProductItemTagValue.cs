using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Tags;

namespace ProjectDigiKala.Models.Products
{
    public class ProductItemTagValue
    {
        public int ProductItemId { get; set; }
        public ProductItem ProductItem { get; set; }
        public int TagValueId { get; set; }
        public TagValue TagValue { get; set; }
    }
}
