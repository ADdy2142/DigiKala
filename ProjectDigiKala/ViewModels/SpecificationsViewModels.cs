using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.ViewModels
{
    public class SpecificationsGroupListViewModel
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }
        public IEnumerable<SpecificationGroup> SpecificationGroups { get; set; }
    }

    public class SpecificationsGroupAddViewModel
    {
        [Required(ErrorMessage = "شناسه گروه یافت نشد.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مشخصه فنی را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مشخصه فنی را مشخص کنید.")]
        public State State { get; set; }
    }

    public class SpecificationsGroupEditViewModel
    {
        [Required(ErrorMessage = "شناسه مشخصه فنی یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "شناسه گروه یافت نشد.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مشخصه فنی را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مشخصه فنی را مشخص کنید.")]
        public State State { get; set; }
    }

    public class SpecificationsGroupDeleteViewModel
    {
        [Required(ErrorMessage = "شناسه مشخصه فنی یافت نشد.")]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Title { get; set; }
    }

    public class SpecificationListViewModel
    {
        public int GroupId { get; set; }
        public IEnumerable<Specification> Specifications { get; set; }
    }

    public class SpecificationAddViewModel
    {
        [Required(ErrorMessage = "شناسه گروه مشخصه فنی یافت نشد.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مشخصه فنی را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مشخصه فنی را مشخص کنید.")]
        public State State { get; set; }
    }

    public class SpecificationEditViewModel
    {
        [Required(ErrorMessage = "شناسه مشخصه فنی یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "شناسه گروه مشخصه فنی یافت نشد.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان مشخصه فنی را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا وضعیت مشخصه فنی را مشخص کنید.")]
        public State State { get; set; }
    }

    public class SpecificationDeleteViewModel
    {
        [Required(ErrorMessage = "شناسه مشخصه فنی یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "شناسه گروه مشخصه فنی یافت نشد.")]
        public int GroupId { get; set; }
        public string Title { get; set; }
        public string GroupTitle { get; set; }
    }
}
