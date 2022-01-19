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
    public class KeyPointRepository : IKeyPointRepository
    {
        private ApplicationDbContext _context;

        public KeyPointRepository(ApplicationDbContext context) => _context = context;

        public void Add(KeyPoint keyPoint) => _context.keyPoints.Add(keyPoint);

        public async Task AddAsync(KeyPoint keyPoint) => await _context.AddAsync(keyPoint);

        public void Delete(KeyPoint keyPoint) => _context.keyPoints.Remove(keyPoint);

        public async Task DeleteAsync(KeyPoint keyPoint)
        {
            await Task.Run(() =>
            {
                Delete(keyPoint);
            });
        }

        public void DeleteById(int id)
        {
            var keyPoint = GetKeyPointById(id);
            Delete(keyPoint);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var keyPoint = await GetKeyPointByIdAsync(id);
            await DeleteAsync(keyPoint);
        }

        public KeyPoint GetKeyPointById(int id) => _context.keyPoints.Include(k => k.Creator).Include(k => k.LastModifier).Include(k => k.Product).SingleOrDefault(k => k.Id == id);

        public async Task<KeyPoint> GetKeyPointByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                return GetKeyPointById(id);
            });
        }

        public IEnumerable<KeyPoint> GetKeyPoints(int productId) => _context.keyPoints.Where(k => k.Product.Id == productId).Include(k => k.Creator).Include(k => k.LastModifier).Include(k => k.Product);

        public async Task<IEnumerable<KeyPoint>> GetKeyPointsAsync(int productId) => await _context.keyPoints.Where(k => k.Product.Id == productId).Include(k => k.Creator).Include(k => k.LastModifier).Include(k => k.Product).ToAsyncEnumerable().ToList();

        public bool IsBelongToProduct(Product product, KeyPoint keyPoint) => keyPoint.Product.Id == product.Id;

        public async Task<bool> IsBelongToProductAsync(Product product, KeyPoint keyPoint)
        {
            return await Task.Run(() =>
            {
                return IsBelongToProduct(product, keyPoint);
            });
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(KeyPoint keyPoint)
        {
            var mainKeyPoint = GetKeyPointById(keyPoint.Id);
            mainKeyPoint.Title = keyPoint.Title;
            mainKeyPoint.Type = keyPoint.Type;
            mainKeyPoint.LastModifier = keyPoint.LastModifier;
            mainKeyPoint.LastModifyDate = keyPoint.LastModifyDate;
            mainKeyPoint.State = keyPoint.State;
        }

        public async Task UpdateAsync(KeyPoint keyPoint)
        {
            var mainKeyPoint = await GetKeyPointByIdAsync(keyPoint.Id);
            mainKeyPoint.Title = keyPoint.Title;
            mainKeyPoint.Type = keyPoint.Type;
            mainKeyPoint.LastModifier = keyPoint.LastModifier;
            mainKeyPoint.LastModifyDate = keyPoint.LastModifyDate;
            mainKeyPoint.State = keyPoint.State;
        }
    }
}
