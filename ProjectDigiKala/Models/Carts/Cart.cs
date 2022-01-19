using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Carts
{
    public class Cart
    {
        public int Id { get; set; }
        public Operator Customer { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
    }
}
