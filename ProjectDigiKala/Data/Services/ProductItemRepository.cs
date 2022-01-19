using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Services
{
    public class ProductItemRepository : IProductItemRepository
    {
        private ApplicationDbContext _context;

        public ProductItemRepository(ApplicationDbContext context) => _context = context;

        public void Add(ProductItem productItem) => _context.ProductItems.Add(productItem);

        public async Task AddAsync(ProductItem productItem) => await _context.ProductItems.AddAsync(productItem);

        public void AddRange(IEnumerable<ProductItem> productItems) => _context.ProductItems.AddRange(productItems);

        public async Task AddRangeAsync(IEnumerable<ProductItem> productItems) =>
            await _context.ProductItems.AddRangeAsync(productItems);

        public void AddProductItemTagValues(IEnumerable<ProductItemTagValue> productItemTagValues) =>
            _context.ProductItemTagValues.AddRange(productItemTagValues);

        public async Task AddProductItemTagValuesAsync(IEnumerable<ProductItemTagValue> productItemTagValues) =>
            await _context.ProductItemTagValues.AddRangeAsync(productItemTagValues);

        public void Update(ProductItem productItem)
        {
            var mainProductItem = GetProductItemById(productItem.Id);
            mainProductItem.State = productItem.State;
            mainProductItem.LastModifier = productItem.LastModifier;
            mainProductItem.LastModifyDate = productItem.LastModifyDate;
            mainProductItem.Discount = productItem.Discount;
            mainProductItem.Price = productItem.Price;
            mainProductItem.Quantity = productItem.Quantity;
        }

        public async Task UpdateAsync(ProductItem productItem)
        {
            var mainProductItem = await GetProductItemByIdAsync(productItem.Id);
            mainProductItem.State = productItem.State;
            mainProductItem.LastModifier = productItem.LastModifier;
            mainProductItem.LastModifyDate = productItem.LastModifyDate;
            mainProductItem.Discount = productItem.Discount;
            mainProductItem.Price = productItem.Price;
            mainProductItem.Quantity = productItem.Quantity;
        }

        public void UpdateRange(IEnumerable<ProductItem> productItems)
        {
            foreach (var productItem in productItems)
            {
                var mainProductItem = GetProductItemById(productItem.Id);
                mainProductItem.State = productItem.State;
                mainProductItem.LastModifier = productItem.LastModifier;
                mainProductItem.LastModifyDate = productItem.LastModifyDate;
                mainProductItem.Discount = productItem.Discount;
                mainProductItem.Price = productItem.Price;
                mainProductItem.Quantity = productItem.Quantity;
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<ProductItem> productItems)
        {
            foreach (var productItem in productItems)
            {
                var mainProductItem = await GetProductItemByIdAsync(productItem.Id);
                mainProductItem.State = productItem.State;
                mainProductItem.LastModifier = productItem.LastModifier;
                mainProductItem.LastModifyDate = productItem.LastModifyDate;
                mainProductItem.Discount = productItem.Discount;
                mainProductItem.Price = productItem.Price;
                mainProductItem.Quantity = productItem.Quantity;
            }
        }

        public void Delete(ProductItem productItem) => _context.ProductItems.Remove(productItem);

        public async Task DeleteAsync(ProductItem productItem)
        {
            await Task.Run(() =>
            {
                Delete(productItem);
            });
        }

        public void DeleteById(int id)
        {
            var productItem = GetProductItemById(id);
            Delete(productItem);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var productItem = await GetProductItemByIdAsync(id);
            await DeleteAsync(productItem);
        }

        public void DeleteProductItemTagValues(IEnumerable<ProductItemTagValue> productItemTagValues) => _context
            .ProductItemTagValues
            .RemoveRange(productItemTagValues);

        public async Task DeleteProductItemTagValuesAsync(IEnumerable<ProductItemTagValue> productItemTagValues) => await Task.Run(() =>
        {
            DeleteProductItemTagValues(productItemTagValues);
        });

        public ProductItem GetProductItemById(int id) => _context
            .ProductItems
            .Include(p => p.Creator)
            .Include(p => p.LastModifier)
            .Include(p => p.Product)
            .Include(p => p.ProductItemTagValues)
            .ThenInclude(p => p.TagValue)
            .SingleOrDefault(p => p.Id == id);

        public async Task<ProductItem> GetProductItemByIdAsync(int id) =>
            await _context
                .ProductItems
                .Include(p => p.Creator)
                .Include(p => p.LastModifier)
                .Include(p => p.Product)
                .Include(p => p.ProductItemTagValues)
                .ThenInclude(p => p.TagValue)
                .SingleOrDefaultAsync(p => p.Id == id);

        public IEnumerable<ProductItem> GetProductItems(int id = 0, int productId = 0, State state = State.Enable)
        {
            if (id == 0)
                return _context
                    .ProductItems
                    .Include(p => p.Creator)
                    .Include(p => p.LastModifier)
                    .Include(p => p.Product)
                    .Include(p => p.ProductItemTagValues)
                    .Where(p => p.State == state && p.Product.Id == productId);
            else
                return _context
                    .ProductItems
                    .Include(p => p.Creator)
                    .Include(p => p.LastModifier)
                    .Include(p => p.Product)
                    .Include(p => p.ProductItemTagValues)
                    .Where(p => p.Id == id);
        }

        public async Task<IEnumerable<ProductItem>> GetProductItemsAsync(int id = 0, int productId = 0, State state = State.Enable)
        {
            if (id == 0)
                return await _context
                    .ProductItems
                    .Include(p => p.Creator)
                    .Include(p => p.LastModifier)
                    .Include(p => p.Product)
                    .Include(p => p.ProductItemTagValues)
                    .ThenInclude(p => p.TagValue)
                    .ThenInclude(t => t.Tag)
                    .Where(p => p.State == state && p.Product.Id == productId)
                    .ToAsyncEnumerable()
                    .ToList();
            else
                return await _context
                    .ProductItems
                    .Include(p => p.Creator)
                    .Include(p => p.LastModifier)
                    .Include(p => p.Product)
                    .Include(p => p.ProductItemTagValues)
                    .Where(p => p.Id == id)
                    .ToAsyncEnumerable()
                    .ToList();
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
