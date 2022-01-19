using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjectDigiKala.ViewModels;

namespace ProjectDigiKala.Controllers
{
    [Authorize]
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
        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var claims = await _userManager.GetClaimsAsync(user);
                        if (claims.Any(c => c.Value == "Customer"))
                            return Redirect(returnUrl ?? "/");
                        else
                            return RedirectToAction(nameof(AccessDenied));
                    }
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                if (ModelState.IsValid)
                {
                    var customer = new Operator
                    {
                        Name = model.FirstName.Trim(),
                        Family = model.LastName.Trim(),
                        Email = model.Email.Trim(),
                        UserName = model.Email.Trim()
                    };

                    var result = await _userManager.CreateAsync(customer, model.Password.Trim());
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddClaimAsync(customer, new Claim("UserType", "Customer"));
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Profile");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("UserExists", "این ایمیل قبلا ثبت شده. برای ورود به سایت لطفا از طریق دکمه ورود اقدام کنید.");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
