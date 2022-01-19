using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Tags;

namespace ProjectDigiKala.ViewModels
{
    public class DeleteTagViewModel
    {
        [Required(ErrorMessage = "شناسه برچسب یافت نشد.")]
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class ValuesViewModel
    {
        public Tag Tag { get; set; }
        public IEnumerable<TagValue> TagValues { get; set; }
    }

    public class AddValueViewModel
    {
        [Required(ErrorMessage = "برچسب مورد نظر یافت نشد.")]
        public int TagId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مقدار را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مقدار برچسب را تعیین کنید.")]
        public State State { get; set; }
    }

    public class EditValueViewModel
    {
        [Required(ErrorMessage = "شناسه برچسب یافت نشد.")]
        public int TagId { get; set; }
        [Required(ErrorMessage = "شناسه مقدار برچسب یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مقدار را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مقدار برچسب را تعیین کنید.")]
        public State State { get; set; }
    }

    public class DeleteValueViewModel
    {
        [Required(ErrorMessage = "شناسه برچسب یافت نشد.")]
        public int TagId { get; set; }
        [Required(ErrorMessage = "شناسه مقدار برچسب یافت نشد.")]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
