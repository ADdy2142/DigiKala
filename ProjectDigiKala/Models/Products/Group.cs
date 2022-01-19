using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان گروه را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "نام گروه طولانی است.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا اسلاگ گروه را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "نام اسلاگ طولانی است.")]
        public string Slug { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
        public IEnumerable<SpecificationGroup> SpecificationGroups { get; set; }
    }
}
