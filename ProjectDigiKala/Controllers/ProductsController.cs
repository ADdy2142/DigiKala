using Microsoft.AspNetCore.Mvc;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.ViewModels;

namespace ProjectDigiKala.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;
        private IGroupRepository _groupRepository;

        public ProductsController(IProductRepository productRepository, IGroupRepository groupRepository)
        {
            _productRepository = productRepository;
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(string search, double? fromPrice, double? toPrice, int[] brands, int[] specs, string keyword, int? pageSize)
        {
            var products = await _productRepository.GetProductsAsync(string.IsNullOrEmpty(search) ? "" : search, fromPrice, toPrice, brands, specs);
            var model = products.Select(p => new ProductsListViewModel()
            {
                Id = p.Id,
                ProductItemId = p.ProductItems.First().Quantity == 0 ? 0 : p.ProductItems.First().Id,
                PrimaryTitle = p.PrimaryTitle,
                SecondaryTitle = p.SecondaryTitle,
                Price = p.ProductItems.First().Quantity == 0 ? null : p.ProductItems.First().Price.ToString("N0"),
                Photo = p.Photo,
                Brand = p.Brand,
                Group = p.Group
            });

            ViewBag.SearchText = search;
            ViewBag.ProductsListTitle = string.IsNullOrWhiteSpace(search) ? null : $"جستجو برای '{search}'";
            return View(model);
        }

        [HttpGet]
        [Route("[controller]/[action]/{id:int?}/{productName?}", Name = "productItem")]
        public async Task<IActionResult> Item(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var product = await _productRepository.GetProductDetailsAsync(id.Value);
            if (product == null)
                return new NotFoundResult();

            return View(product);
        }

        [HttpGet]
        [Route("[controller]/[action]/{groupId}/{groupTitle}")]
        public async Task<IActionResult> Group(int? groupId)
        {
            if (groupId == null)
                return new BadRequestResult();

            var group = await _groupRepository.GetGroupByIdAsync(groupId.Value);
            if (group == null)
                return new NotFoundResult();

            var products = await _productRepository.GetProductsAsync(group.Id);
            var model = products.Select(p => new ProductsListViewModel()
            {
                Id = p.Id,
                ProductItemId = p.ProductItems.First().Quantity == 0 ? 0 : p.ProductItems.First().Id,
                PrimaryTitle = p.PrimaryTitle,
                SecondaryTitle = p.SecondaryTitle,
                Price = p.ProductItems.First().Quantity == 0 ? null : p.ProductItems.First().Price.ToString("N0"),
                Photo = p.Photo,
                Brand = p.Brand,
                Group = p.Group
            });

            ViewBag.ProductsListTitle = $"محصولات گروه {group.Title}";
            return View(nameof(List), model);
        }
    }
}
