using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Orders
{
    public enum PaymentState : byte
    {
        [Description("پرداخت شده")]
        Paid = 1,
        [Description("پرداخت نشده")]
        Unpaid = 2
    }
}
