using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Models.Products
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "متن کامنت را وارد کنید.")]
        public string Text { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public Customer Customer { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
