using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public class SpecificationValue
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
    }
}
