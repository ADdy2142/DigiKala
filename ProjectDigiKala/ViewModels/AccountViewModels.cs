using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Remotion.Linq.Clauses.ResultOperators;

namespace ProjectDigiKala.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }

    public class SignUpViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکسان نیستند.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید.")]
        public string LastName { get; set; }
    }
}
