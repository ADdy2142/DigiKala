using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Tags
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان برچسب را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت برچسب را تعیین کنید.")]
        public State State { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public IEnumerable<TagValue> TagValues { get; set; }
    }
}
