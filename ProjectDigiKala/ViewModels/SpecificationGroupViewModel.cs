using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.ViewModels
{
    public class SpecificationGroupViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public GroupViewModel Group { get; set; }
        public IEnumerable<SpecificationViewModel> Specifications { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
    }
}
