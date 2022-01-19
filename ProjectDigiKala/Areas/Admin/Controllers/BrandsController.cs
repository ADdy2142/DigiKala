using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandsController : BaseController
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;
        private IBrandRepository _brandRepository;
        private IHostingEnvironment _hostingEnvironment;

        public BrandsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IBrandRepository brandRepository, IHostingEnvironment hostingEnvironment) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _userManager = userManager;
            _signInManager = signInManager;
            _brandRepository = brandRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<ViewResult> List(int? id, byte? state, string title = "", string slug = "")
        {
            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            var brands = await _brandRepository.GetBrandsAsync(id, state, title, slug);
            return View(brands);
        }

        [HttpGet]
        public ViewResult Add()
        {
            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن برند جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Brand brand, IFormFile BrandPhoto)
        {
            if (brand.State == 0)
                ModelState.AddModelError("InvalidState", "لطفا وضعیت برند را مشخص کنید.");
            if (BrandPhoto == null)
                ModelState.AddModelError("PhotoRequired", "لطفا لوگو برند مورد نظر را تعیین کنید.");

            if (ModelState.IsValid)
            {
                if (BrandPhoto != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(BrandPhoto.FileName);
                    brand.Photo = fileName;

                    if (!Directory.Exists(ImagesLocation))
                        Directory.CreateDirectory(ImagesLocation);

                    string fullPath = Path.Combine(ImagesLocation, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        await BrandPhoto.CopyToAsync(fileStream);
                }
                brand.Creator = Operator;
                brand.CreateDate = DateTime.Now;
                var addBrand = _brandRepository.AddAsync(brand);
                await addBrand;
                await _brandRepository.SaveAsync();

                return RedirectToAction(nameof(List));
            }

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن برند جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(brand);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return new NotFoundResult();

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "مشخصات برند"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var brand = await _brandRepository.GetBrandByIdAsync(id.Value);

            if (brand == null)
                return new NotFoundResult();

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش برند {brand.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand brand, IFormFile BrandPhoto)
        {
            if (ModelState.IsValid)
            {
                var selectedBrand = await _brandRepository.GetBrandByIdAsync(brand.Id);

                if (BrandPhoto != null)
                {
                    if (!Directory.Exists(ImagesLocation))
                        Directory.CreateDirectory(ImagesLocation);

                    string oldFilePath = Path.Combine(ImagesLocation, selectedBrand.Photo);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(BrandPhoto.FileName);
                    selectedBrand.Photo = fileName;

                    string fullPath = Path.Combine(ImagesLocation, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        await BrandPhoto.CopyToAsync(fileStream);
                }

                selectedBrand.LastModifier = Operator;
                selectedBrand.LastModifyDate = DateTime.Now;
                await _brandRepository.UpdateAsync(selectedBrand);
                await _brandRepository.SaveAsync();

                return RedirectToAction(nameof(List));
            }

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش {brand.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var brand = await _brandRepository.GetBrandByIdAsync(id.Value);

            if (brand == null)
                return new NotFoundResult();

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف برند {brand.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Brand brand)
        {
            var selectedBrand = await _brandRepository.GetBrandByIdAsync(brand.Id);
            if (selectedBrand != null)
            {
                string imageLocation = selectedBrand.Photo;
                var deleteBrand = _brandRepository.DeleteAsync(selectedBrand);
                await deleteBrand;
                await _brandRepository.SaveAsync();

                if (!Directory.Exists(ImagesLocation))
                    Directory.CreateDirectory(ImagesLocation);

                string oldFilePath = Path.Combine(ImagesLocation, selectedBrand.Photo);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                return RedirectToAction(nameof(List));
            }

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برندها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف برند {brand.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(brand);
        }
    }
}
