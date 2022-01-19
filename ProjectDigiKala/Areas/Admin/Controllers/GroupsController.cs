using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GroupsController : BaseController
    {
        private UserManager<Operator> _userManager;
        private SignInManager<Operator> _signInManager;
        private IGroupRepository _groupRepository;

        public GroupsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IGroupRepository groupRepository, IHostingEnvironment hostingEnvironment) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _userManager = userManager;
            _signInManager = signInManager;
            _groupRepository = groupRepository;
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
                    Title = "لیست گروه ها"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            var groups = await _groupRepository.GetGroupsAsync(id, state, title, slug);
            return View(groups);
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن گروه جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Group group)
        {
            if (group.State == 0)
                ModelState.AddModelError("InvalidState", "لطفا وضعیت گروه را مشخص کنید.");

            if (ModelState.IsValid)
            {
                group.Creator = Operator;
                group.CreateDate = DateTime.Now;
                var addGroup = _groupRepository.AddAsync(group);
                await addGroup;
                await _groupRepository.SaveAsync();

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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = "افزودن گروه جدید"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(id.Value);

            if (group == null)
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = "مشخصات گروه"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(id.Value);

            if (group == null)
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش {group.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                group.LastModifier = Operator;
                group.LastModifyDate = DateTime.Now;
                await _groupRepository.UpdateAsync(group);
                await _groupRepository.SaveAsync();

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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش {group.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(id.Value);

            if (group == null)
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف {group.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Group group)
        {
            var selectedGroup = await _groupRepository.GetGroupByIdAsync(group.Id);
            if (selectedGroup != null)
            {
                var deleteGroup = _groupRepository.DeleteAsync(selectedGroup);
                await deleteGroup;
                await _groupRepository.SaveAsync();

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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"حذف {group.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(group);
        }
    }
}
