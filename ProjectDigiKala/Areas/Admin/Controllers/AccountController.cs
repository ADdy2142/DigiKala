using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;

        public AccountController(UserManager<Operator> userManager, SignInManager<Operator> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(string username, string password, bool rememberMe = false, string returnUrl = null)
        {
            var user = await _userManager.FindByNameAsync(username);
            ViewBag.ReturnUrl = returnUrl;

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return Redirect("/Admin/");
                }
            }

            ViewBag.ErrorMessage = "نام کاربری یا کلمه عبور اشتباه وارد شده است.";
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
