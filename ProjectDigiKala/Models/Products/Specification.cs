using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public class Specification
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public SpecificationGroup SpecificationGroup { get; set; }
        public SpecificationValue SpecificationValue { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
    }
}
