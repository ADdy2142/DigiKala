using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Tags;
using ProjectDigiKala.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagsController : BaseController
    {
        private ITagRepository _tagRepository;
        private ITagValueRepository _tagValueRepository;

        public TagsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment, ITagRepository tagRepository, ITagValueRepository tagValueRepository) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _tagRepository = tagRepository;
            _tagValueRepository = tagValueRepository;
        }

        [HttpGet]
        public async Task<ViewResult> List(int? id, string title = "", byte? state = null)
        {
            var tags = await _tagRepository.GetTagsAsync(id, title, state);

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برچسب ها"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(tags);
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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن برچسب جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var op = Operator;
                tag.Creator = op;
                tag.CreateDate = DateTime.Now;

                var add = _tagRepository.AddAsync(tag);
                Task.WaitAll(add);
                await add;
                await _tagRepository.SaveAsync();

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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن برچسب جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(tag);
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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = "مشخصات برچسب"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundResult();

            var tag = await _tagRepository.GetTagByIdAsync(id.Value);

            if (tag == null)
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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش برچسب {string.Empty}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var selectedTag = await _tagRepository.GetTagByIdAsync(tag.Id);
                var op = Operator;
                selectedTag.LastModifier = op;
                selectedTag.LastModifyDate = DateTime.Now;
                selectedTag.State = tag.State;
                selectedTag.Title = tag.Title;

                var editTag = _tagRepository.UpdateAsync(selectedTag);
                var save = _tagRepository.SaveAsync();

                await editTag;
                await save;

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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش برچسب {string.Empty}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var tag = await _tagRepository.GetTagByIdAsync(id.Value);

            if (tag == null)
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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف برچسب {tag.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;

            var model = new DeleteTagViewModel()
            {
                Id = tag.Id,
                Title = tag.Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteTagViewModel model)
        {
            var selectedTag = await _tagRepository.GetTagByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                var deleteTag = _tagRepository.DeleteAsync(selectedTag);
                var save = _tagRepository.SaveAsync();

                await deleteTag;
                await save;

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
                    Title = "لیست برچسب ها",
                    Url = $"/{Area}/Products/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف برچسب {selectedTag.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(selectedTag);
        }

        [HttpGet]
        public async Task<IActionResult> Values(int? tagId, int id = 0, string title = "", State state = State.Enable)
        {
            if (tagId == null)
                return new BadRequestResult();

            var tag = await _tagRepository.GetTagByIdAsync(tagId.Value);
            if (tag == null)
                return new NotFoundResult();

            var tagValues = await _tagValueRepository.GetTagValuesAsync(tagId.Value, id, string.IsNullOrEmpty(title) ? "" : title, state);
            if (tagValues == null)
                return new NotFoundResult();

            var model = new ValuesViewModel { Tag = tag, TagValues = tagValues };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tag.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddValue(int? tagId)
        {
            if (tagId == null)
                return new BadRequestResult();

            var tag = await _tagRepository.GetTagByIdAsync(tagId.Value);
            if (tag == null)
                return new NotFoundResult();

            var model = new AddValueViewModel { TagId = tag.Id, State = State.Enable, Title = "" };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن مقدار جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddValue(AddValueViewModel model)
        {
            var tag = await _tagRepository.GetTagByIdAsync(model.TagId);

            if (ModelState.IsValid)
            {
                if (tag == null)
                    return new BadRequestResult();

                var tagValue = new TagValue();

                tagValue.State = model.State;
                tagValue.Title = model.Title;
                var op = Operator;
                tagValue.Creator = op;
                tagValue.CreateDate = DateTime.Now;
                tagValue.Tag = tag;

                var addTagValue = _tagValueRepository.AddAsync(tagValue);
                await addTagValue;

                await _tagValueRepository.SaveAsync();

                return RedirectToAction(nameof(Values), new { tagId = tag.Id });
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
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن مقدار جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Value(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var tagValue = await _tagValueRepository.GetTagValueByIdAsync(id.Value);
            if (tagValue == null)
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
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tagValue.Tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tagValue.Tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"مشخصات مقدار {tagValue.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(tagValue);
        }

        [HttpGet]
        public async Task<IActionResult> EditValue(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var tagValue = await _tagValueRepository.GetTagValueByIdAsync(id.Value);
            if (tagValue == null)
                return new NotFoundResult();

            var model = new EditValueViewModel { TagId = tagValue.Tag.Id, Id = tagValue.Id, Title = tagValue.Title, State = tagValue.State };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tagValue.Tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tagValue.Tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش مقدار {tagValue.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditValue(EditValueViewModel model)
        {
            var tagValue = await _tagValueRepository.GetTagValueByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                var op = Operator;
                var tagValueModel = new TagValue
                {
                    Id = model.Id,
                    Title = model.Title,
                    State = model.State,
                    LastModifier = op,
                    LastModifyDate = DateTime.Now
                };

                var editTagValue = _tagValueRepository.UpdateAsync(tagValueModel);
                await editTagValue;
                await _tagValueRepository.SaveAsync();

                return RedirectToAction(nameof(Values), new { tagId = tagValue.Tag.Id });
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
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tagValue.Tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tagValue.Tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش مقدار {tagValue.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteValue(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var tagValue = await _tagValueRepository.GetTagValueByIdAsync(id.Value);
            if (tagValue == null)
                return new NotFoundResult();

            var model = new DeleteValueViewModel { Id = tagValue.Id, Title = tagValue.Title, TagId = tagValue.Tag.Id };

            Breadcrumbs = new List<Breadcrumb>()
            {
                new Breadcrumb()
                {
                    Title = "صفحه اصلی",
                    Url = $"/{Area}/Home/Index"
                },
                new Breadcrumb()
                {
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tagValue.Tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tagValue.Tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف مقدار {tagValue.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteValue(DeleteValueViewModel model)
        {
            var tagValue = await _tagValueRepository.GetTagValueByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                var deleteTagValue = _tagValueRepository.DeleteAsync(tagValue);
                await deleteTagValue;
                await _tagValueRepository.SaveAsync();

                return RedirectToAction(nameof(Values), new { tagId = tagValue.Tag.Id });
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
                    Title = "لیست برچسب ها",
                    Url= $"/{Area}/Tags/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مقادیر برچسب {tagValue.Tag.Title}",
                    Url = $"/{Area}/Tags/Values?tagId={tagValue.Tag.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف مقدار {tagValue.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }
    }
}
