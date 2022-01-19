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
    public class SpecificationValueRepository : ISpecificationValueRepository
    {
        private ApplicationDbContext _context;

        public SpecificationValueRepository(ApplicationDbContext context) => _context = context;

        public void Add(SpecificationValue specificationValue) => _context.Add(specificationValue);

        public async Task AddAsync(SpecificationValue specificationValue) =>
            await _context.AddAsync(specificationValue);

        public async Task AddRangeAsync(IEnumerable<SpecificationValue> specificationValues) => await _context.AddRangeAsync(specificationValues);

        public void Update(SpecificationValue specificationValue)
        {
            var mainSpecificationValue = GetSpecificationValueById(specificationValue.Id);
            mainSpecificationValue.Value = specificationValue.Value;
            mainSpecificationValue.State = specificationValue.State;
            mainSpecificationValue.LastModifier = specificationValue.LastModifier;
            mainSpecificationValue.LastModifyDate = specificationValue.LastModifyDate;
        }

        public async Task UpdateAsync(SpecificationValue specificationValue)
        {
            var mainSpecificationValue = await GetSpecificationValueByIdAsync(specificationValue.Id);
            mainSpecificationValue.Value = specificationValue.Value;
            mainSpecificationValue.State = specificationValue.State;
            mainSpecificationValue.LastModifier = specificationValue.LastModifier;
            mainSpecificationValue.LastModifyDate = specificationValue.LastModifyDate;
        }

        public async Task UpdateRangeAsync(IEnumerable<SpecificationValue> specificationValues)
        {
            await Task.Run(() =>
            {
                _context.UpdateRange(specificationValues);
            });
        }

        public void Delete(SpecificationValue specificationValue) =>
            _context.SpecificationValues.Remove(specificationValue);

        public async Task DeleteAsync(SpecificationValue specificationValue)
        {
            await Task.Run(() =>
            {
                Delete(specificationValue);
            });
        }

        public void DeleteById(int id)
        {
            var specificationValue = GetSpecificationValueById(id);
            Delete(specificationValue);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var specificationValue = await GetSpecificationValueByIdAsync(id);
            await DeleteAsync(specificationValue);
        }

        public void DeleteRange(IEnumerable<SpecificationValue> specificationValues) =>
            _context.RemoveRange(specificationValues);

        public async Task DeleteRangeAsync(IEnumerable<SpecificationValue> specificationValues) => await Task.Run(() =>
        {
            DeleteRange(specificationValues);
        });

        public SpecificationValue GetSpecificationValueById(int id) => _context
            .SpecificationValues
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.Product)
            .Include(s => s.Specification)
            .SingleOrDefault(s => s.Id == id);

        public async Task<SpecificationValue> GetSpecificationValueByIdAsync(int id) => await _context
            .SpecificationValues
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.Product)
            .Include(s => s.Specification)
            .SingleOrDefaultAsync(s => s.Id == id);

        public IEnumerable<SpecificationValue> GetSpecificationValues(int id = 0, string title = "", State state = State.Enable)
        {
            if (id == 0)
                return _context
                    .SpecificationValues
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Product)
                    .Include(s => s.Specification)
                    .Where(s => s.Value.Contains(title) || s.State == state)
                    .ToList();
            else
                return _context
                    .SpecificationValues
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Product)
                    .Include(s => s.Specification)
                    .Where(s => s.Id == id)
                    .ToList();
        }

        public async Task<IEnumerable<SpecificationValue>> GetSpecificationValuesAsync(int id = 0, string title = "", State state = State.Enable)
        {
            if (id == 0)
                return await _context
                    .SpecificationValues
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Product)
                    .Include(s => s.Specification)
                    .Where(s => s.Value.Contains(title) || s.State == state)
                    .ToAsyncEnumerable()
                    .ToList();
            else
                return await _context
                    .SpecificationValues
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Product)
                    .Include(s => s.Specification)
                    .Where(s => s.Id == id)
                    .ToAsyncEnumerable()
                    .ToList();
        }

        public IEnumerable<SpecificationValue> GetSpecificationValues(int productId) => _context
                .SpecificationValues
                .Include(s => s.Product)
                .Where(s => s.Product.Id == productId);

        public async Task<IEnumerable<SpecificationValue>> GetSpecificationValuesAsync(int productId) => await _context
            .SpecificationValues
            .Include(s => s.Product)
            .Where(s => s.Product.Id == productId)
            .ToAsyncEnumerable()
            .ToList();

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
