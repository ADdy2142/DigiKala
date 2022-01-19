using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Orders
{
    public enum ShippingType : byte
    {
        [Description("پست پیشتاز (15,000 تومان)")]
        immediately = 1,
        [Description("پست معمولی (12,000 تومان)")]
        Common = 2
    }
}
