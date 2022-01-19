using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Orders
{
    public enum PaymentType : byte
    {
        [Description("کارت به کارت")]
        CartToCart = 1,
        [Description("واریز به حساب")]
        Variz = 2,
        [Description("پرداخت درب منزل")]
        Home = 2,
        [Description("پرداخت آنلاین")]
        Online = 4
    }
}
