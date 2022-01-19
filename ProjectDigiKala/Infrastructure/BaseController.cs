using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Infrastructure
{
    // استاد میخواستن یه چیزی در مورد متدهای OnActionExecuted و OnActionExecuting که در کلاس Controller قرار داره بگن، ولی متاسفانه ترجیح دادن از این موضوع صرف نظر کنن. :|
    public class BaseController : Controller
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;
        private IHostingEnvironment _hostingEnvironment;

        public BaseController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Breadcrumb> Breadcrumbs { get; set; }
        public string Area { get; set; }
        public Operator Operator => GetOperator().Result;
        //public Operator Operator => GetOperator().GetAwaiter().GetResult();
        public string ImagesLocation => _hostingEnvironment.WebRootPath + "\\images\\";

        private async Task<Operator> GetOperator() => await _userManager.FindByNameAsync(User.Identity.Name);
    }
}
