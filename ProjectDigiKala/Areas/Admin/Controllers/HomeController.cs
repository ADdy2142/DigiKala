using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;

        public HomeController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }
    }
}
