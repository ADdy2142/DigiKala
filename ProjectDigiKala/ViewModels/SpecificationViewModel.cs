using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.ViewModels
{
    public class SpecificationViewModel
    {
        public int Id { get; set; }
        public int SpecificationValueId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
