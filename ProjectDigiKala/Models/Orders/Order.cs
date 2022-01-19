using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Profile;

namespace ProjectDigiKala.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public Operator Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public ShippingType ShippingType { get; set; }
        public PaymentType PaymentType { get; set; }
        public Address Address { get; set; }
        public PaymentState PaymentState { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
