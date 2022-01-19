using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان کالا را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "عنوان کالا طولانی است.")]
        public string PrimaryTitle { get; set; }
        [Required(ErrorMessage = "لطفا عنوان دوم کالا را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "عنوان دوم کالا طولانی است.")]
        public string SecondaryTitle { get; set; }
        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید.")]
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public Group Group { get; set; }
        public State State { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public IEnumerable<KeyPoint> KeyPoints { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public string Photo { get; set; }
        public IEnumerable<ProductItem> ProductItems { get; set; }
        public IEnumerable<SpecificationValue> SpecificationValues { get; set; }
    }
}
