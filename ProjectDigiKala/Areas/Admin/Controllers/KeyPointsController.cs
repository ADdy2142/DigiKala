using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KeyPointsController : BaseController
    {
        private IKeyPointRepository _KeyPointRepository;
        private IProductRepository _productRepository;

        public KeyPointsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment, IKeyPointRepository keyPointRepository, IProductRepository productRepository) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _KeyPointRepository = keyPointRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(int? productId)
        {
            if (productId == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);

            if (product == null)
                return new NotFoundResult();

            var keyPoints = await _KeyPointRepository.GetKeyPointsAsync(productId.Value);

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {product.PrimaryTitle}"
                }
            };

            ViewBag.ProductId = product.Id;
            ViewBag.ProductName = product.PrimaryTitle;
            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(keyPoints);
        }

        [HttpGet]
        public async Task<IActionResult> Add(int? productId)
        {
            if (productId == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);
            var model = new AddKeyPointViewModel()
            {
                ProductId = product.Id
            };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={product.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddKeyPointViewModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);

            if (ModelState.IsValid)
            {
                var keyPoint = new KeyPoint();

                keyPoint.Title = model.Title;
                keyPoint.Type = model.Type;
                keyPoint.State = model.State;
                var op = Operator;
                keyPoint.Creator = op;
                keyPoint.CreateDate = DateTime.Now;
                keyPoint.Product = product;

                var addKeyPoint = _KeyPointRepository.AddAsync(keyPoint);
                await addKeyPoint;
                await _KeyPointRepository.SaveAsync();

                return RedirectToAction(nameof(List), new { productId = product.Id });
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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={product.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int? productId)
        {
            if (id == null || productId == null)
                return new BadRequestResult();

            var keyPoint = await _KeyPointRepository.GetKeyPointByIdAsync(id.Value);
            var product = await _productRepository.GetProductByIdAsync(productId.Value);

            if (keyPoint == null || product == null)
                return new NotFoundResult();

            if (!await _KeyPointRepository.IsBelongToProductAsync(product, keyPoint))
                return new BadRequestResult();

            var model = new EditKeyPointViewModel()
            {
                Id = keyPoint.Id,
                ProductId = product.Id,
                State = keyPoint.State,
                Title = keyPoint.Title,
                Type = keyPoint.Type
            };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {keyPoint.Product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={keyPoint.Product.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditKeyPointViewModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);

            if (ModelState.IsValid)
            {
                var keyPoint = await _KeyPointRepository.GetKeyPointByIdAsync(model.Id);

                if (await _KeyPointRepository.IsBelongToProductAsync(product, keyPoint))
                {
                    var op = Operator;
                    keyPoint.LastModifier = op;
                    keyPoint.LastModifyDate = DateTime.Now;
                    keyPoint.State = model.State;
                    keyPoint.Title = model.Title;
                    keyPoint.Type = model.Type;

                    var editKeyPoint = _KeyPointRepository.UpdateAsync(keyPoint);
                    await editKeyPoint;
                    await _KeyPointRepository.SaveAsync();

                    return RedirectToAction(nameof(List), new { productId = product.Id });
                }
                else
                    ModelState.AddModelError("FailedToSave", "مشکلی در ویرایش نکته کلیدی به وجود آمد.");
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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={product.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, int? productId)
        {
            if (id == null || productId == null)
                return new BadRequestResult();

            var keyPoint = await _KeyPointRepository.GetKeyPointByIdAsync(id.Value);
            var product = await _productRepository.GetProductByIdAsync(productId.Value);

            if (keyPoint == null || product == null)
                return new NotFoundResult();

            if (!await _KeyPointRepository.IsBelongToProductAsync(product, keyPoint))
                return new BadRequestResult();

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {keyPoint.Product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={keyPoint.Product.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(keyPoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(KeyPoint keyPoint)
        {
            var selectedKeyPoint = await _KeyPointRepository.GetKeyPointByIdAsync(keyPoint.Id);
            if (selectedKeyPoint != null)
            {
                var deleteKeyPoint = _KeyPointRepository.DeleteAsync(selectedKeyPoint);
                await deleteKeyPoint;
                await _KeyPointRepository.SaveAsync();

                return RedirectToAction(nameof(List), new { productId = selectedKeyPoint.Product.Id });
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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست نکات کلیدی {selectedKeyPoint.Product.PrimaryTitle}",
                    Url = $"/{Area}/KeyPoints/List?productId={selectedKeyPoint.Product.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف نکته کلیدی"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(keyPoint);
        }
    }
}
