using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Tags;

namespace ProjectDigiKala.ViewModels
{
    public class ItemsProductItemViewModel
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public byte Quantity { get; set; }
        public State State { get; set; }
        public string Creator { get; set; }
        public string CreateDate { get; set; }
        public string LastModifier { get; set; }
        public string LastModifyDate { get; set; }
        public IEnumerable<ProductItemTagValue> ProductItemTagValues { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class AddProductItemViewModel
    {
        [Required(ErrorMessage = "لطفا قیمت قلم کالا را وارد کنید.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "لطفا میزان تخفیف قلم کالا را مشخص کنید.")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "تعداد موجود از این قلم کالا وارد نشده است.")]
        public byte Quantity { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت قلم کالا را مشخص کنید.")]
        public State State { get; set; }
        public List<TagsDictionary> Tags { get; set; }
        [Required(ErrorMessage = "لطفا مقدار قلم کالا را انتخاب کنید.")]
        public Dictionary<int, int> ReturnIds { get; set; }
        [Required(ErrorMessage = "شناسه محصول یافت نشد.")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class EditProductItemViewModel
    {
        [Required(ErrorMessage = "شناسه قلم کالا یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا قیمت قلم کالا را وارد کنید.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "لطفا میزان تخفیف قلم کالا را مشخص کنید.")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "تعداد موجود از این قلم کالا وارد نشده است.")]
        public byte Quantity { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت قلم کالا را مشخص کنید.")]
        public State State { get; set; }
        public List<TagsDictionary> Tags { get; set; }
        [Required(ErrorMessage = "لطفا مقدار قلم کالا را انتخاب کنید.")]
        public Dictionary<int, int> ReturnIds { get; set; }
        [Required(ErrorMessage = "شناسه محصول یافت نشد.")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class DeleteProductItemViewModel
    {
        [Required(ErrorMessage = "شناسه قلم کالا یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "شناسه محصول یافت نشد.")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class TagsDictionary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public SelectList TagValues { get; set; }
    }

    public class ProductItemTagValueViewModel
    {

    }
}
