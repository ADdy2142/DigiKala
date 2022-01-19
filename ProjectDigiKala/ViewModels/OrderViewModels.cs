using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Orders;
using ProjectDigiKala.Models.Profile;

namespace ProjectDigiKala.ViewModels
{
    public class OrderIndexViewModel
    {
        public string TotalPrice { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
