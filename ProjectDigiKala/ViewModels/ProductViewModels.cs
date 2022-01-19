using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "عنوان اصلی را وارد کنید.")]
        public string PrimaryTitle { get; set; }
        [Required(ErrorMessage = "عنوان دوم را وارد کنید.")]
        public string SecondaryTitle { get; set; }
        [Required(ErrorMessage = "توضیحات را وارد کنید.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت محصول را مشخص کنید.")]
        public State State { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Groups { get; set; }
        public int? BrandId { get; set; }
        public int? GroupId { get; set; }
    }

    public class EditProductViewModel
    {
        [Required(ErrorMessage = "شناسه محصول یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "عنوان اصلی را وارد کنید.")]
        public string PrimaryTitle { get; set; }
        [Required(ErrorMessage = "عنوان دوم را وارد کنید.")]
        public string SecondaryTitle { get; set; }
        [Required(ErrorMessage = "توضیحات را وارد کنید.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت محصول را مشخص کنید.")]
        public State State { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Groups { get; set; }
        public int? BrandId { get; set; }
        public int? GroupId { get; set; }
    }

    public class DeleteProductViewModel
    {
        [Required(ErrorMessage = "شناسه محصول یافت نشد.")]
        public int Id { get; set; }
        public string PrimaryTitle { get; set; }
    }

    public class SpecificationsViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SpecificationGroupViewModel> SpecificationGroups { get; set; }
    }

    public class ProductsListViewModel
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string PrimaryTitle { get; set; }
        public string SecondaryTitle { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
        public Group Group { get; set; }
        public Brand Brand { get; set; }
    }
}
