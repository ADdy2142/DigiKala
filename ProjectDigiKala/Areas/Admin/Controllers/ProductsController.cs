using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Infrastructure.ExtensionMethods;
using ProjectDigiKala.Models.Tags;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : BaseController
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;
        private IBrandRepository _brandRepository;
        private IGroupRepository _groupRepository;
        private IProductRepository _productRepository;
        private ISpecificationGroupRepository _specificationGroupRepository;
        private ISpecificationRepository _specificationRepository;
        private ISpecificationValueRepository _specificationValueRepository;
        private ITagRepository _tagRepository;
        private IProductItemRepository _productItemRepository;
        private ITagValueRepository _tagValueRepository;

        public ProductsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment, IBrandRepository brandRepository, IGroupRepository groupRepository, IProductRepository productRepository, ISpecificationGroupRepository specificationGroupRepository, ISpecificationRepository specificationRepository, ISpecificationValueRepository specificationValueRepository, ITagRepository tagRepository, IProductItemRepository productItemRepository, ITagValueRepository tagValueRepository) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _userManager = userManager;
            _signInManager = signInManager;
            _brandRepository = brandRepository;
            _groupRepository = groupRepository;
            _productRepository = productRepository;
            _specificationGroupRepository = specificationGroupRepository;
            _specificationRepository = specificationRepository;
            _specificationValueRepository = specificationValueRepository;
            _tagRepository = tagRepository;
            _productItemRepository = productItemRepository;
            _tagValueRepository = tagValueRepository;
        }

        [HttpGet]
        public async Task<ViewResult> List(int? productId, int? brandId, int? groupId, byte? state, string title = "")
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
                    Title = "لیست کالاها"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            ViewBag.Brands = new SelectList(await _brandRepository.GetBrandsAsync(null, 1), "Id", "Title");
            ViewBag.Groups = new SelectList(await _groupRepository.GetGroupsAsync(null, 1), "Id", "Title");
            var products = await _productRepository.GetProductsAsync(productId, brandId, groupId, state, title);
            return View(products);
        }

        [HttpGet]
        public async Task<ViewResult> Add()
        {
            var model = new AddProductViewModel()
            {
                Brands = new SelectList(await _brandRepository.GetBrandsAsync(null, 1), "Id", "Title"),
                Groups = new SelectList(await _groupRepository.GetGroupsAsync(null, 1), "Id", "Title")
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
                    Title = "افزودن کالای جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddProductViewModel model, IFormFile ProductPhoto)
        {
            if (model.BrandId == null)
                ModelState.AddModelError("InvalidBrandId", "لطفا برند محصول را انتخاب کنید.");
            if (model.GroupId == null)
                ModelState.AddModelError("InvalidGroupId", "لطفا گروه محصول را انتخاب کنید.");
            if (ProductPhoto == null)
                ModelState.AddModelError("PhotoRequiredForProduct", "تصویری برای محصول انتخاب نشده است.");

            if (ModelState.IsValid)
            {
                var product = new Product();

                product.Brand = await _brandRepository.GetBrandByIdAsync(model.BrandId.Value);
                product.Group = await _groupRepository.GetGroupByIdAsync(model.GroupId.Value);
                var op = Operator;
                product.Creator = op;
                product.CreateDate = DateTime.Now;
                product.State = model.State;
                product.Description = model.Description;
                product.PrimaryTitle = model.PrimaryTitle;
                product.SecondaryTitle = model.SecondaryTitle;

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProductPhoto.FileName);
                product.Photo = fileName;

                if (!Directory.Exists(ImagesLocation))
                    Directory.CreateDirectory(ImagesLocation);

                string fullPath = Path.Combine(ImagesLocation, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    await ProductPhoto.CopyToAsync(fileStream);

                var addProduct = _productRepository.AddAsync(product);
                await addProduct;

                _productRepository.SaveAsync().Wait();

                return RedirectToAction(nameof(List));
            }

            model.Brands = new SelectList(await _brandRepository.GetBrandsAsync(null, 1), "Id", "Title");
            model.Groups = new SelectList(await _groupRepository.GetGroupsAsync(null, 1), "Id", "Title");

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
                    Title = "افزودن کالای جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "مشخصات کالا"
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

            var product = await _productRepository.GetProductByIdAsync(id.Value);

            if (product == null)
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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "ویرایش"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            var model = new EditProductViewModel()
            {
                BrandId = product.Brand.Id,
                Brands = new SelectList(await _brandRepository.GetBrandsAsync(null, 1), "Id", "Title"),
                GroupId = product.Group.Id,
                Groups = new SelectList(await _groupRepository.GetGroupsAsync(null, 1), "Id", "Title"),
                Description = product.Description,
                Id = product.Id,
                PrimaryTitle = product.PrimaryTitle,
                SecondaryTitle = product.SecondaryTitle,
                State = product.State
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel model, IFormFile ProductPhoto)
        {
            if (model.BrandId == null)
                ModelState.AddModelError("InvalidBrandId", "لطفا برند محصول را انتخاب کنید.");
            if (model.GroupId == null)
                ModelState.AddModelError("InvalidGroupId", "لطفا گروه محصول را انتخاب کنید.");

            if (ModelState.IsValid)
            {
                var product = await _productRepository.GetProductByIdAsync(model.Id);

                product.Brand = await _brandRepository.GetBrandByIdAsync(model.BrandId.Value);
                product.Group = await _groupRepository.GetGroupByIdAsync(model.GroupId.Value);
                var op = Operator;
                product.LastModifier = op;
                product.LastModifyDate = DateTime.Now;
                product.PrimaryTitle = model.PrimaryTitle;
                product.SecondaryTitle = model.SecondaryTitle;
                product.Description = model.Description;
                product.State = model.State;

                if (ProductPhoto != null)
                {
                    if (!Directory.Exists(ImagesLocation))
                        Directory.CreateDirectory(ImagesLocation);

                    string oldFilePath = Path.Combine(ImagesLocation, product.Photo);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProductPhoto.FileName);
                    product.Photo = fileName;

                    string fullPath = Path.Combine(ImagesLocation, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        await ProductPhoto.CopyToAsync(fileStream);
                }

                var editProduct = _productRepository.UpdateAsync(product);
                await editProduct;

                _productRepository.SaveAsync().Wait();

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
                    Title = "لیست کالاها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "ویرایش"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(id.Value);
            if (product == null)
                return new NotFoundResult();

            var model = new DeleteProductViewModel()
            {
                Id = product.Id,
                PrimaryTitle = product.PrimaryTitle
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
                    Title = "حذف"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteProductViewModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.Id);
            if (product == null)
                ModelState.AddModelError("ProductNotFound", "محصول مورد نظر یافت نشد.");

            if (ModelState.IsValid)
            {
                var specificationValues = await _specificationValueRepository.GetSpecificationValuesAsync(product.Id);
                await _specificationValueRepository.DeleteRangeAsync(specificationValues);
                await _specificationValueRepository.SaveAsync();

                if (!Directory.Exists(ImagesLocation))
                    Directory.CreateDirectory(ImagesLocation);

                string oldFilePath = Path.Combine(ImagesLocation, product.Photo);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                await _productRepository.DeleteAsync(product);
                await _productRepository.SaveAsync();

                return RedirectToAction(nameof(List));
            }

            model.Id = product.Id;
            model.PrimaryTitle = product.PrimaryTitle;

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
                    Title = "حذف"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Specifications(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(id.Value);
            if (product == null)
                return new NotFoundResult();

            var specificationGroups = await _specificationGroupRepository.GetSpecificationGroupsAsync(product.Group.Id);
            specificationGroups = specificationGroups.Where(group => group.State == State.Enable);
            var specificationGroupViewModels = specificationGroups.Select(s => new SpecificationGroupViewModel()
            {
                Id = s.Id,
                Title = s.Title,
                Group = new GroupViewModel()
                {
                    Id = s.Group.Id,
                    Title = s.Group.Title
                },
                Specifications = s.Specifications.Where(spec => spec.State == State.Enable).Select(spec => new SpecificationViewModel()
                {
                    Id = spec.Id,
                    SpecificationValueId = spec.SpecificationValue?.Id ?? 0,
                    Title = spec.Title,
                    Value = spec.SpecificationValue?.Value ?? ""
                }),
                Creator = s.Creator.FullName,
                CreateDate = s.CreateDate,
                LastModifier = s.LastModifier?.FullName ?? "--",
                LastModifyDate = s.LastModifyDate,
                State = s.State
            });

            var model = new SpecificationsViewModel()
            {
                Product = product,
                SpecificationGroups = specificationGroupViewModels
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
                    Title = $"مدیریت مشخصات فنی {string.Empty}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSpecifications(int productId, int[] ids, int[] specificationValuesIds)
        {
            var addValues = new List<SpecificationValue>();
            var editValues = new List<SpecificationValue>();
            int index = 0;

            foreach (var id in ids)
            {
                bool update = specificationValuesIds[index] != 0;

                if (update)
                {
                    string value = Request.Form["Value_" + id];
                    var specificationValue =
                        await _specificationValueRepository.GetSpecificationValueByIdAsync(specificationValuesIds[index]);
                    if (specificationValue.Value != value)
                    {
                        specificationValue.LastModifier = Operator;
                        specificationValue.LastModifyDate = DateTime.Now;
                        specificationValue.Value = value;
                        editValues.Add(specificationValue);
                    }
                }
                else
                {
                    string value = Request.Form["Value_" + id];
                    var spec = new SpecificationValue
                    {
                        Value = value,
                        Creator = Operator,
                        CreateDate = DateTime.Now,
                        State = State.Enable,
                        Product = await _productRepository.GetProductByIdAsync(productId),
                        SpecificationId = id,
                        Specification = await _specificationRepository.GetSpecificationByIdAsync(id)
                    };
                    addValues.Add(spec);
                }

                index++;
            }

            if (addValues.Count > 0)
                await _specificationValueRepository.AddRangeAsync(addValues);

            if (editValues.Count > 0)
                await _specificationValueRepository.UpdateRangeAsync(editValues);

            await _specificationValueRepository.SaveAsync();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        [Route("[area]/[controller]/{productId}/{productName}/[action]")]
        public async Task<IActionResult> Items(int? productId)
        {
            if (productId == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);
            if (product == null)
                return new NotFoundResult();

            var productItems = await _productItemRepository.GetProductItemsAsync(0, product.Id, State.Enable);

            var model = new List<ItemsProductItemViewModel>();

            foreach (var item in productItems)
            {
                model.Add(new ItemsProductItemViewModel()
                {
                    Id = item.Id,
                    Price = item.Price,
                    Discount = item.Discount,
                    Quantity = item.Quantity,
                    State = item.State,
                    Creator = item.Creator.FullName,
                    CreateDate = item.CreateDate?.ToPersianDate(),
                    LastModifier = item.LastModifier?.FullName ?? "--",
                    LastModifyDate = item.LastModifyDate?.ToPersianDate() ?? "--",
                    ProductItemTagValues = item.ProductItemTagValues,
                    ProductId = product.Id,
                    ProductName = product.SecondaryTitle.Replace(' ', '-')
                });
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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            ViewBag.ProductId = product.Id;
            ViewBag.ProductName = product.SecondaryTitle.Replace(' ', '-');
            ViewBag.ProductTitle = product.PrimaryTitle;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/{productId}/{productName}/[action]")]
        public async Task<IActionResult> AddItem(int? productId)
        {
            if (productId == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);
            if (product == null)
                return new NotFoundResult();

            var tags = await _tagRepository.GetTagsAsync(null, string.Empty, (byte)State.Enable);

            var tagsDictionary = new List<TagsDictionary>();
            foreach (var tag in tags)
                tagsDictionary.Add(new TagsDictionary()
                {
                    Id = tag.Id,
                    Title = tag.Title,
                    TagValues = new SelectList(tag.TagValues, "Id", "Title")
                });

            var model = new AddProductItemViewModel()
            {
                State = State.Enable,
                Discount = 0,
                Quantity = 0,
                Tags = tagsDictionary,
                Price = 0,
                ProductId = product.Id,
                ProductName = product.SecondaryTitle
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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "افزودن قلم کالای جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [Route("[area]/[controller]/{productId}/{productName}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddProductItemViewModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            if (product == null)
                ModelState.AddModelError("ProductNotFound", "محصول مورد نظر یافت نشد.");

            if (ModelState.IsValid)
            {
                var productItem = new ProductItem
                {
                    State = model.State,
                    Creator = Operator,
                    CreateDate = DateTime.Now,
                    Discount = model.Discount,
                    Price = model.Price,
                    Product = product,
                    Quantity = model.Quantity
                };

                await _productItemRepository.AddAsync(productItem);
                await _productItemRepository.SaveAsync();

                List<ProductItemTagValue> productItemTagValues = new List<ProductItemTagValue>();
                foreach (var value in model.ReturnIds)
                {
                    var tagValue = await _tagValueRepository.GetTagValueByIdAsync(value.Value);
                    productItemTagValues.Add(new ProductItemTagValue()
                    {
                        ProductItemId = productItem.Id,
                        ProductItem = productItem,
                        TagValueId = tagValue.Id,
                        TagValue = tagValue
                    });
                }

                await _productItemRepository.AddProductItemTagValuesAsync(productItemTagValues);
                await _productItemRepository.SaveAsync();

                return RedirectToAction(nameof(Items));
            }

            var tags = await _tagRepository.GetTagsAsync(null, string.Empty, (byte)State.Enable);

            var tagsDictionary = new List<TagsDictionary>();
            foreach (var tag in tags)
                tagsDictionary.Add(new TagsDictionary()
                {
                    Id = tag.Id,
                    Title = tag.Title,
                    TagValues = new SelectList(tag.TagValues, "Id", "Title")
                });

            model.ProductId = product.Id;
            model.ProductName = product.SecondaryTitle;
            model.Tags = tagsDictionary;

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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "افزودن قلم کالای جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/{productId}/{productName}/[action]/{id}")]
        public async Task<IActionResult> EditItem(int? productId, int? id)
        {
            if (id == null || productId == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);
            if (product == null)
                return new NotFoundResult();

            var productItem = await _productItemRepository.GetProductItemByIdAsync(id.Value);
            if (productItem == null)
                return new NotFoundResult();

            var tags = await _tagRepository.GetTagsAsync(null, string.Empty, (byte)State.Enable);

            var tagsDictionary = new List<TagsDictionary>();
            foreach (var tag in tags)
            {
                var tagDictionary = new TagsDictionary();
                tagDictionary.Id = tag.Id;
                tagDictionary.Title = tag.Title;
                foreach (var item in tag.TagValues)
                {
                    var tagValue = productItem.ProductItemTagValues.SingleOrDefault(p => p.TagValueId == item.Id);
                    if (tagValue != null)
                    {
                        tagDictionary.TagValues = new SelectList(tag.TagValues, "Id", "Title", tagValue.TagValueId);
                        break;
                    }
                    else
                    {
                        tagDictionary.TagValues = new SelectList(tag.TagValues, "Id", "Title");
                        break;
                    }
                }

                tagsDictionary.Add(tagDictionary);
            }

            var model = new EditProductItemViewModel
            {
                Id = productItem.Id,
                Price = productItem.Price,
                Discount = productItem.Discount,
                Quantity = productItem.Quantity,
                State = productItem.State,
                Tags = tagsDictionary,
                ReturnIds = new Dictionary<int, int>(),
                ProductId = product.Id,
                ProductName = product.SecondaryTitle.Replace(' ', '-')
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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "ویرایش قلم کالا"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [Route("[area]/[controller]/{productId}/{productName}/[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(EditProductItemViewModel model)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            var productItem = await _productItemRepository.GetProductItemByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                productItem.Discount = model.Discount;
                productItem.LastModifier = Operator;
                productItem.LastModifyDate = DateTime.Now;
                productItem.Price = model.Price;
                productItem.Quantity = model.Quantity;
                productItem.State = model.State;

                await _productItemRepository.DeleteProductItemTagValuesAsync(productItem.ProductItemTagValues);

                List<ProductItemTagValue> productItemTagValues = new List<ProductItemTagValue>();
                foreach (var value in model.ReturnIds)
                {
                    var tagValue = await _tagValueRepository.GetTagValueByIdAsync(value.Value);
                    productItemTagValues.Add(new ProductItemTagValue()
                    {
                        ProductItemId = productItem.Id,
                        ProductItem = productItem,
                        TagValueId = tagValue.Id,
                        TagValue = tagValue
                    });
                }

                await _productItemRepository.AddProductItemTagValuesAsync(productItemTagValues);
                await _productItemRepository.UpdateAsync(productItem);
                await _productItemRepository.SaveAsync();

                return RedirectToAction(nameof(Items));
            }

            var tags = await _tagRepository.GetTagsAsync(null, string.Empty, (byte)State.Enable);

            var tagsDictionary = new List<TagsDictionary>();
            foreach (var tag in tags)
            {
                var tagDictionary = new TagsDictionary();
                tagDictionary.Id = tag.Id;
                tagDictionary.Title = tag.Title;
                foreach (var item in tag.TagValues)
                {
                    var tagValue = productItem.ProductItemTagValues.SingleOrDefault(p => p.TagValueId == item.Id);
                    if (tagValue != null)
                    {
                        tagDictionary.TagValues = new SelectList(tag.TagValues, "Id", "Title", tagValue.TagValueId);
                        break;
                    }
                    else
                    {
                        tagDictionary.TagValues = new SelectList(tag.TagValues, "Id", "Title");
                        break;
                    }
                }

                tagsDictionary.Add(tagDictionary);
            }

            model.Tags = tagsDictionary;

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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "ویرایش قلم کالا"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/{productId}/{productName}/[action]/{id}")]
        public async Task<IActionResult> DeleteItem(int? productId, int? id)
        {
            if (productId == null || id == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductByIdAsync(productId.Value);
            if (product == null)
                return new NotFoundResult();

            var productItem = await _productItemRepository.GetProductItemByIdAsync(id.Value);
            if (productItem == null)
                return new NotFoundResult();

            var model = new DeleteProductItemViewModel();
            model.Id = productItem.Id;
            model.ProductId = product.Id;
            model.ProductName = product.SecondaryTitle.Replace(' ', '-');

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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "حذف قلم کالا"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [Route("[area]/[controller]/{productId}/{productName}/[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(DeleteProductItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productItem = await _productItemRepository.GetProductItemByIdAsync(model.Id);

                await _productItemRepository.DeleteProductItemTagValuesAsync(productItem.ProductItemTagValues);
                await _productItemRepository.DeleteAsync(productItem);
                await _productItemRepository.SaveAsync();

                return RedirectToAction(nameof(Items));
            }

            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            model.ProductName = product.SecondaryTitle.Replace(' ', '-');

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
                    Title = $"لیست اقلام کالای {product.PrimaryTitle}",
                    Url = $"/{Area}/Products/{product.Id}/{product.SecondaryTitle.Replace(' ', '-')}/Items"
                },
                new Breadcrumb()
                {
                    Title = "حذف قلم کالا"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }
    }
}
