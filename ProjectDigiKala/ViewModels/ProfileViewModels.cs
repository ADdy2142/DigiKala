using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.ViewModels
{
    public class AddAddressViewModel
    {
        [Required(ErrorMessage = "لطفا استان محل سکونت خود را مشخص کنید.")]
        public string Province { get; set; }
        [Required(ErrorMessage = "لطفا شهر محل سکونت خود را مشخص کنید.")]
        public string City { get; set; }
        [Required(ErrorMessage = "لطفا آدرس منزل/محل کار خود را وارد کنید.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید.")]
        public string Phone { get; set; }
    }

    public class EditAddressViewModel
    {
        [Required(ErrorMessage = "شناسه نشانی مورد نظر یافت نشد.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا استان محل سکونت خود را مشخص کنید.")]
        public string Province { get; set; }
        [Required(ErrorMessage = "لطفا شهر محل سکونت خود را مشخص کنید.")]
        public string City { get; set; }
        [Required(ErrorMessage = "لطفا آدرس منزل/محل کار خود را وارد کنید.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید.")]
        public string Phone { get; set; }
    }
}
