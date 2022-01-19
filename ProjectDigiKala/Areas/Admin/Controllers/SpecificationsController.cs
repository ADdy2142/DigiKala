using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Infrastructure;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Breadcrumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.ViewModels;
using Remotion.Linq.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDigiKala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SpecificationsController : BaseController
    {
        private ISpecificationGroupRepository _specificationGroupRepository;
        private ISpecificationRepository _specificationRepository;
        private ISpecificationValueRepository _specificationValueRepository;
        private IGroupRepository _groupRepository;

        public SpecificationsController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHostingEnvironment hostingEnvironment, ISpecificationGroupRepository specificationGroupRepository, ISpecificationRepository specificationRepository, ISpecificationValueRepository specificationValueRepository, IGroupRepository groupRepository) : base(userManager, signInManager, hostingEnvironment)
        {
            Area = "Admin";
            _specificationGroupRepository = specificationGroupRepository;
            _specificationRepository = specificationRepository;
            _specificationValueRepository = specificationValueRepository;
            _groupRepository = groupRepository;
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> List(int? groupId, int id = 0, string title = "", State state = State.Enable)
        {
            if (groupId == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(groupId.Value);
            if (group == null)
                return new NotFoundResult();

            var specificationGroup = await _specificationGroupRepository
                .GetSpecificationGroupsAsync(group.Id, id, string.IsNullOrEmpty(title) ? "" : title, state);

            var model = new SpecificationsGroupListViewModel
            {
                GroupId = group.Id,
                GroupTitle = group.Title,
                SpecificationGroups = specificationGroup.ToList()
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {group.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> ListSubGroups(int? groupId, int id = 0, string title = "", State state = State.Enable)
        {
            if (groupId == null)
                return new BadRequestResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(groupId.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var specifications = await _specificationRepository.GetSpecificationsAsync(groupId.Value, id,
                string.IsNullOrEmpty(title) ? "" : title, state);

            var model = new SpecificationListViewModel()
            {
                GroupId = specificationGroup.Id,
                Specifications = specifications.ToList()
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}"
                }
            };

            ViewBag.SpecificationGroupTitle = specificationGroup.Title;
            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> Add(int? groupId)
        {
            if (groupId == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(groupId.Value);
            if (group == null)
                return new NotFoundResult();

            var model = new SpecificationsGroupAddViewModel { GroupId = groupId.Value };

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
                    Title = $"لیست مشخصات فنی گروه {group.Title}",
                    Url = $"/{Area}/Specifications/List/{group.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن مشخصه"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> Add(SpecificationsGroupAddViewModel model)
        {
            var group = await _groupRepository.GetGroupByIdAsync(model.GroupId);
            if (ModelState.IsValid)
            {
                var op = Operator;
                var specificationGroup = new SpecificationGroup
                {
                    Title = model.Title,
                    State = model.State,
                    Group = group,
                    Creator = op,
                    CreateDate = DateTime.Now
                };

                await _specificationGroupRepository.AddAsync(specificationGroup);
                await _specificationGroupRepository.SaveAsync();

                return RedirectToAction(nameof(List), new { groupId = group.Id });
            }

            model.GroupId = group.Id;

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
                    Title = $"لیست مشخصات فنی گروه {group.Title}",
                    Url = $"/{Area}/Specifications/List/{group.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن مشخصه"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> AddSubGroup(int? groupId)
        {
            if (groupId == null)
                return new BadRequestResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(groupId.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var model = new SpecificationAddViewModel()
            {
                GroupId = specificationGroup.Id
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن زیرشاخه"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{groupId?}")]
        public async Task<IActionResult> AddSubGroup(SpecificationAddViewModel model)
        {
            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(model.GroupId);

            if (ModelState.IsValid)
            {
                var specification = new Specification()
                {
                    Title = model.Title,
                    State = model.State,
                    Creator = Operator,
                    CreateDate = DateTime.Now,
                    SpecificationGroup = specificationGroup
                };

                await _specificationRepository.AddAsync(specification);
                await _specificationRepository.SaveAsync();

                return RedirectToAction(nameof(ListSubGroups), new { groupId = specificationGroup.Id });
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
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = "افزودن زیرشاخه"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> Edit(int? id, int? groupId)
        {
            if (id == null || groupId == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(groupId.Value);
            if (group == null)
                return new NotFoundResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(id.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var model = new SpecificationsGroupEditViewModel
            {
                Id = specificationGroup.Id,
                GroupId = groupId.Value,
                Title = specificationGroup.Title,
                State = specificationGroup.State
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {group.Title}",
                    Url = $"/{Area}/Specifications/List/{group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش مشخصه فنی {specificationGroup.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> Edit(SpecificationsGroupEditViewModel model)
        {
            var group = await _groupRepository.GetGroupByIdAsync(model.GroupId);
            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                var op = Operator;
                var specificationGroupModel = new SpecificationGroup
                {
                    Id = model.Id,
                    Title = model.Title,
                    State = model.State,
                    LastModifier = op,
                    LastModifyDate = DateTime.Now
                };

                await _specificationGroupRepository.UpdateAsync(specificationGroupModel);
                await _specificationGroupRepository.SaveAsync();

                return RedirectToAction(nameof(List), new { groupId = group.Id });
            }

            model.Id = specificationGroup.Id;
            model.GroupId = group.Id;

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
                    Title = $"لیست مشخصات فنی گروه {group.Title}",
                    Url = $"/{Area}/Specifications/List/{group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش مشخصه فنی {specificationGroup.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> EditSubGroup(int? id, int? groupId)
        {
            if (id == null || groupId == null)
                return new BadRequestResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(groupId.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var specification = await _specificationRepository.GetSpecificationByIdAsync(id.Value);
            if (specification == null)
                return new NotFoundResult();

            var model = new SpecificationEditViewModel()
            {
                Id = specification.Id,
                GroupId = specificationGroup.Id,
                Title = specification.Title,
                State = specification.State
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش زیرشاخه {specification.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> EditSubGroup(SpecificationEditViewModel model)
        {
            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(model.GroupId);
            var specification = await _specificationRepository.GetSpecificationByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                var mainSpecification = new Specification()
                {
                    Id = specification.Id,
                    Title = model.Title,
                    State = model.State,
                    LastModifier = Operator,
                    LastModifyDate = DateTime.Now
                };

                await _specificationRepository.UpdateAsync(mainSpecification);
                await _specificationRepository.SaveAsync();

                return RedirectToAction(nameof(ListSubGroups), new { groupId = specificationGroup.Id });
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
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"ویرایش زیرشاخه {specification.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> Delete(int? id, int? groupId)
        {
            if (id == null || groupId == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(groupId.Value);
            if (group == null)
                return new NotFoundResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(id.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var model = new SpecificationsGroupDeleteViewModel()
            {
                Id = specificationGroup.Id,
                GroupId = groupId.Value,
                Title = specificationGroup.Title
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {group.Title}",
                    Url = $"/{Area}/Specifications/List/{group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف مشخصه فنی {specificationGroup.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> Delete(SpecificationsGroupDeleteViewModel model)
        {
            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                await _specificationGroupRepository.DeleteAsync(specificationGroup);
                await _specificationGroupRepository.SaveAsync();

                return RedirectToAction(nameof(List), new { groupId = specificationGroup.Group.Id });
            }

            model.Id = specificationGroup.Id;
            model.GroupId = specificationGroup.Group.Id;
            model.Title = specificationGroup.Title;

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
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف مشخصه فنی {specificationGroup.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> DeleteSubGroup(int? id, int? groupId)
        {
            if (id == null || groupId == null)
                return new BadRequestResult();

            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(groupId.Value);
            if (specificationGroup == null)
                return new NotFoundResult();

            var specification = await _specificationRepository.GetSpecificationByIdAsync(id.Value);
            if (specification == null)
                return new NotFoundResult();

            var model = new SpecificationDeleteViewModel()
            {
                Id = specification.Id,
                GroupId = specificationGroup.Id,
                Title = specification.Title,
                GroupTitle = specificationGroup.Title
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
                    Title = "لیست گروه ها",
                    Url = $"/{Area}/Groups/List"
                },
                new Breadcrumb()
                {
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف زیرشاخه {specification.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/[controller]/[action]/{id?}/{groupId?}")]
        public async Task<IActionResult> DeleteSubGroup(SpecificationDeleteViewModel model)
        {
            var specificationGroup = await _specificationGroupRepository.GetSpecificationGroupByIdAsync(model.GroupId);
            var specification = await _specificationRepository.GetSpecificationByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                await _specificationRepository.DeleteAsync(specification);
                await _specificationRepository.SaveAsync();

                return RedirectToAction(nameof(ListSubGroups), new { groupId = specificationGroup.Id });
            }

            model.Id = specification.Id;
            model.GroupId = specificationGroup.Id;
            model.Title = specification.Title;
            model.GroupTitle = specificationGroup.Title;

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
                    Title = $"لیست مشخصات فنی گروه {specificationGroup.Group.Title}",
                    Url = $"/{Area}/Specifications/List/{specificationGroup.Group.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"لیست زیرشاخه های مشخصه فنی {specificationGroup.Title}",
                    Url = $"/{Area}/Specifications/ListSubGroups/{specificationGroup.Id}"
                },
                new Breadcrumb()
                {
                    Title = $"حذف زیرشاخه {specification.Title}"
                }
            };

            ViewBag.Breadcrumbs = Breadcrumbs;
            return View(model);
        }
    }
}
