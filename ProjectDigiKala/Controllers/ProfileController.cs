using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Profile;
using ProjectDigiKala.ViewModels;

namespace ProjectDigiKala.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private UserManager<Operator> _userManager;
        private IAddressRepository _addressRepository;

        public ProfileController(UserManager<Operator> userManager, IAddressRepository addressRepository)
        {
            _userManager = userManager;
            _addressRepository = addressRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Addresses()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = await _addressRepository.GetAddressesAsync(user.Id);
            return View(model);
        }

        [HttpGet]
        [Route("[controller]/Addresses/Add")]
        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        [Route("[controller]/Addresses/Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    await _addressRepository.AddAsync(new Address()
                    {
                        City = model.City,
                        Customer = user,
                        Location = model.Location,
                        Phone = model.Phone,
                        Province = model.Province
                    });
                    await _addressRepository.SaveAsync();

                    return RedirectToAction(nameof(Addresses));
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("[controller]/Addresses/Edit/{id:int?}")]
        public IActionResult EditAddress(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var address = _addressRepository.GetAddressById(id.Value);
            if (address == null)
                return new NotFoundResult();

            var model = new EditAddressViewModel()
            {
                City = address.City,
                Id = address.Id,
                Location = address.Location,
                Phone = address.Phone,
                Province = address.Province
            };

            return View(model);
        }

        [HttpPost]
        [Route("[controller]/Addresses/Edit/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(EditAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = await _addressRepository.GetAddressByIdAsync(model.Id);
                if (address != null)
                {
                    address.City = model.City;
                    address.Location = model.Location;
                    address.Phone = model.Phone;
                    address.Province = model.Province;

                    await _addressRepository.UpdateAsync(address);
                    await _addressRepository.SaveAsync();

                    return RedirectToAction(nameof(Addresses));
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var address = await _addressRepository.GetAddressByIdAsync(id.Value);
            if (address == null)
                return new NotFoundResult();

            await _addressRepository.DeleteAsync(address);
            await _addressRepository.SaveAsync();

            return RedirectToAction(nameof(Addresses));
        }
    }
}
