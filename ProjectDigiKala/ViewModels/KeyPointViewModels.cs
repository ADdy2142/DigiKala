using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.ViewModels
{
    public class AddKeyPointViewModel
    {
        [Required(ErrorMessage = "شناسه محصول صحیح نمی باشد.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "لطفا عنوان نکته کلیدی را وارد کنید.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا نوع نکته کلیدی را تعیین کنید.")]
        public KeyPointType Type { get; set; }

        [Required(ErrorMessage = "لطفا وضعیت نکته کلیدی را مشخص کنید.")]
        public State State { get; set; }
    }

    public class EditKeyPointViewModel
    {
        [Required(ErrorMessage = "شناسه نکته کلیدی صحیح نمی باشد.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "شناسه محصول صحیح نمی باشد.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "لطفا عنوان نکته کلیدی را وارد کنید.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا نوع نکته کلیدی را تعیین کنید.")]
        public KeyPointType Type { get; set; }

        [Required(ErrorMessage = "لطفا وضعیت نکته کلیدی را مشخص کنید.")]
        public State State { get; set; }
    }
}
