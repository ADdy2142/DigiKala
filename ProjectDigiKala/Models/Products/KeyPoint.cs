using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public enum KeyPointType : byte
    {
        Positive = 1,
        Negative = 2
    }

    public class KeyPoint
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان نکته کلیدی را وارد کنید.")]
        public string Title { get; set; }
        public Product Product { get; set; }
        public KeyPointType Type { get; set; }
        public Operator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public Operator LastModifier { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public State State { get; set; }
    }
}
