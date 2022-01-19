using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Services
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) => _context = context;

        public void Add(Product product) => _context.Products.Add(product);

        public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);

        public void Delete(Product product) => _context.Products.Remove(product);

        public async Task DeleteAsync(Product product)
        {
            await Task.Run(() =>
            {
                Delete(product);
            });
        }

        public void DeleteById(int id)
        {
            var product = GetProductById(id);
            Delete(product);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            await DeleteAsync(product);
        }

        public Product GetProductById(int id) => _context
            .Products
            .Include(p => p.Brand)
            .Include(p => p.Comments)
            .Include(p => p.Creator)
            .Include(p => p.Group)
            .Include(p => p.KeyPoints)
            .Include(p => p.LastModifier)
            .Include(p => p.ProductItems)
            .SingleOrDefault(p => p.Id == id);

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                return GetProductById(id);
            });
        }

        /*
         * .Include(b => b.Brand).Include(b => b.Brand).ThenInclude(o => o.Creator).Include(g => g.Group).ThenInclude(o => o.Creator).Include(o => o.Creator).Include(o => o.LastModifier).Include(k => k.KeyPoints).ThenInclude(k => k.Creator).Include(c => c.Comments).ThenInclude(c => c.Product)
         */

        public IEnumerable<Product> GetProducts(int? productId, int? brandId, int? groupId, byte? state, string title = "")
        {
            if (productId != null)
                return _context.Products.Where(p => p.Id == productId.Value).Include(p => p.Brand).Include(p => p.Comments).Include(p => p.Creator).Include(p => p.Group).Include(p => p.KeyPoints).Include(p => p.LastModifier).ToList();

            if (string.IsNullOrEmpty(title))
                title = string.Empty;

            var list = _context.Products.Where(p => p.Brand.Id == brandId || p.Group.Id == groupId || p.PrimaryTitle.Contains(title) || p.SecondaryTitle.Contains(title)).Include(p => p.Brand).Include(p => p.Comments).Include(p => p.Creator).Include(p => p.Group).Include(p => p.KeyPoints).Include(p => p.LastModifier).ToList();

            if (state != null)
                list = list.Where(p => p.State == (State)state).ToList();

            return list;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int? productId, int? brandId, int? groupId, byte? state, string title = "")
        {
            var list = await _context.Products.Include(p => p.Brand).Include(p => p.Comments).Include(p => p.Creator).Include(p => p.Group).Include(p => p.KeyPoints).Include(p => p.LastModifier).ToAsyncEnumerable().ToList();

            if (productId != null)
                list = list.Where(p => p.Id == productId.Value).ToList();

            if (brandId != null)
                list = list.Where(p => p.Brand.Id == brandId.Value).ToList();

            if (groupId != null)
                list = list.Where(p => p.Group.Id == groupId.Value).ToList();

            if (state != null)
                list = list.Where(p => p.State == (State)state).ToList();

            if (!string.IsNullOrEmpty(title))
                list = list.Where(p => p.PrimaryTitle.Contains(title) || p.SecondaryTitle.Contains(title)).ToList();

            return list;
        }

        public IEnumerable<Product> GetProducts(string keyword, double? fromPrice, double? toPrice, int[] brands, int[] specifications)
        {
            var list = _context
                .Products
                .Include(p => p.Brand)
                .Include(p => p.Group)
                .ThenInclude(g => g.SpecificationGroups)
                .ThenInclude(s => s.Specifications)
                .Include(p => p.ProductItems)
                .Where(p =>
                    (p.PrimaryTitle.Contains(keyword) || p.SecondaryTitle.Contains(keyword) || p.Brand.Title.Contains(keyword) || p.Group.Title.Contains(keyword)) && p.State == State.Enable && p.ProductItems.Any());

            if (fromPrice != null)
                list = list.Where(p => p.ProductItems.Any(productItem => productItem.Price >= fromPrice.Value));

            if (toPrice != null)
                list = list.Where(p => p.ProductItems.Any(productItem => productItem.Price <= toPrice.Value));

            if (brands.Length > 0)
                list = list.Where(p => brands.Contains(p.Brand.Id));

            if (specifications.Length > 0)
                list = list.Where(p =>
                    p.Group.SpecificationGroups.Any(s =>
                        s.Specifications.Any(spec => specifications.Contains(spec.Id))));

            return list;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string keyword, double? fromPrice, double? toPrice,
            int[] brands, int[] specifications)
        {
            var list = await _context
                .Products
                .Include(p => p.Brand)
                .Include(p => p.Group)
                .ThenInclude(g => g.SpecificationGroups)
                .ThenInclude(s => s.Specifications)
                .Include(p => p.ProductItems)
                .Where(p =>
                    (p.PrimaryTitle.Contains(keyword) || p.SecondaryTitle.Contains(keyword) ||
                     p.Brand.Title.Contains(keyword) || p.Group.Title.Contains(keyword)) && p.State == State.Enable && p.ProductItems.Any())
                .ToAsyncEnumerable()
                .ToList();

            if (fromPrice != null)
                list = list.Where(p => p.ProductItems.Any(productItem => productItem.Price >= fromPrice.Value)).ToList();

            if (toPrice != null)
                list = list.Where(p => p.ProductItems.Any(productItem => productItem.Price <= toPrice.Value)).ToList();

            if (brands.Length > 0)
                list = list.Where(p => brands.Contains(p.Brand.Id)).ToList();

            if (specifications.Length > 0)
                list = list.Where(p =>
                    p.Group.SpecificationGroups.Any(s =>
                        s.Specifications.Any(spec => specifications.Contains(spec.Id)))).ToList();

            return list;
        }

        public IEnumerable<Product> GetProducts(int groupId) => _context
            .Products
            .Include(p => p.Brand)
            .Include(p => p.Group)
            .ThenInclude(g => g.SpecificationGroups)
            .ThenInclude(s => s.Specifications)
            .Include(p => p.ProductItems)
            .Where(p => p.Group.Id == groupId && p.State == State.Enable && p.ProductItems.Any());

        public async Task<IEnumerable<Product>> GetProductsAsync(int groupId) => await _context
            .Products
            .Include(p => p.Brand)
            .Include(p => p.Group)
            .ThenInclude(g => g.SpecificationGroups)
            .ThenInclude(s => s.Specifications)
            .Include(p => p.ProductItems)
            .Where(p => p.Group.Id == groupId && p.State == State.Enable && p.ProductItems.Any())
            .ToAsyncEnumerable()
            .ToList();

        public Product GetProductDetails(int productId) => _context
            .Products
            .Include(p => p.Brand)
            .Include(p => p.Group)
            .ThenInclude(g => g.SpecificationGroups)
            .ThenInclude(specGroup => specGroup.Specifications)
            .ThenInclude(spec => spec.SpecificationValue)
            .Include(p => p.ProductItems)
            .ThenInclude(i => i.ProductItemTagValues)
            .ThenInclude(t => t.TagValue)
            .ThenInclude(tag => tag.Tag)
            .Include(p => p.SpecificationValues)
            .ThenInclude(specValue => specValue.Specification)
            .ThenInclude(spec => spec.SpecificationGroup)
            .Include(p => p.KeyPoints)
            .SingleOrDefault(p => p.Id == productId);

        public async Task<Product> GetProductDetailsAsync(int productId) => await _context
            .Products
            .Include(p => p.Brand)
            .Include(p => p.Group)
            .ThenInclude(g => g.SpecificationGroups)
            .ThenInclude(specGroup => specGroup.Specifications)
            .ThenInclude(spec => spec.SpecificationValue)
            .Include(p => p.ProductItems)
            .ThenInclude(i => i.ProductItemTagValues)
            .ThenInclude(t => t.TagValue)
            .ThenInclude(tag => tag.Tag)
            .Include(p => p.KeyPoints)
            .SingleOrDefaultAsync(p => p.Id == productId);

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(Product product)
        {
            var mainProduct = GetProductById(product.Id);
            mainProduct.Brand = product.Brand;
            mainProduct.Comments = product.Comments;
            mainProduct.Description = product.Description;
            mainProduct.Group = product.Group;
            mainProduct.KeyPoints = product.KeyPoints;
            mainProduct.LastModifier = product.LastModifier;
            mainProduct.LastModifyDate = product.LastModifyDate;
            mainProduct.PrimaryTitle = product.PrimaryTitle;
            mainProduct.SecondaryTitle = product.SecondaryTitle;
            mainProduct.State = product.State;
            mainProduct.Photo = product.Photo;
        }

        public async Task UpdateAsync(Product product)
        {
            await Task.Run(() =>
            {
                Update(product);
            });
        }
    }
}
