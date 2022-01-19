using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Models.Carts
{
    public class CartItem
    {
        public int Id { get; set; }
        public Cart Cart { get; set; }
        public ProductItem ProductItem { get; set; }
        public int Quantity { get; set; }
    }
}
