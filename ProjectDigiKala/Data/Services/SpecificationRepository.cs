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
    public class SpecificationRepository : ISpecificationRepository
    {
        private ApplicationDbContext _context;

        public SpecificationRepository(ApplicationDbContext context) => _context = context;

        public void Add(Specification specification) => _context.Specifications.Add(specification);

        public async Task AddAsync(Specification specification) => await _context.Specifications.AddAsync(specification);

        public void Update(Specification specification)
        {
            var mainSpecification = GetSpecificationById(specification.Id);
            mainSpecification.State = specification.State;
            mainSpecification.LastModifier = specification.LastModifier;
            mainSpecification.LastModifyDate = specification.LastModifyDate;
            mainSpecification.Title = specification.Title;
        }

        public async Task UpdateAsync(Specification specification)
        {
            var mainSpecification = await GetSpecificationByIdAsync(specification.Id);
            mainSpecification.State = specification.State;
            mainSpecification.LastModifier = specification.LastModifier;
            mainSpecification.LastModifyDate = specification.LastModifyDate;
            mainSpecification.Title = specification.Title;
        }

        public void Delete(Specification specification) => _context.Specifications.Remove(specification);

        public async Task DeleteAsync(Specification specification)
        {
            await Task.Run(() =>
            {
                Delete(specification);
            });
        }

        public void DeleteById(int id)
        {
            var specification = GetSpecificationById(id);
            Delete(specification);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var specification = await GetSpecificationByIdAsync(id);
            await DeleteAsync(specification);
        }

        public Specification GetSpecificationById(int id) => _context
            .Specifications
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.SpecificationGroup)
            .Include(s => s.SpecificationValue)
            .SingleOrDefault(s => s.Id == id);

        public async Task<Specification> GetSpecificationByIdAsync(int id) => await _context
            .Specifications
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.SpecificationGroup)
            .Include(s => s.SpecificationValue)
            .SingleOrDefaultAsync(s => s.Id == id);

        public IEnumerable<Specification> GetSpecifications(int groupId, int id, string title, State state)
        {
            if (id == 0)
                return _context
                    .Specifications
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.SpecificationGroup)
                    .Include(s => s.SpecificationValue)
                    .Where(s => s.SpecificationGroup.Id == groupId && (s.Title.Contains(title) || s.State == state))
                    .ToList();
            else
                return _context
                    .Specifications
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.SpecificationGroup)
                    .Include(s => s.SpecificationValue)
                    .Where(s => s.SpecificationGroup.Id == groupId && s.Id == id)
                    .ToList();
        }

        public async Task<IEnumerable<Specification>> GetSpecificationsAsync(int groupId, int id, string title, State state)
        {
            if (id == 0)
                return await _context
                    .Specifications
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.SpecificationGroup)
                    .Include(s => s.SpecificationValue)
                    .Where(s => s.SpecificationGroup.Id == groupId && (s.Title.Contains(title) || s.State == state))
                    .ToAsyncEnumerable()
                    .ToList();
            else
                return await _context
                    .Specifications
                    .Include(s => s.Creator)
                    .Include(s => s.LastModifier)
                    .Include(s => s.SpecificationGroup)
                    .Include(s => s.SpecificationValue)
                    .Where(s => s.SpecificationGroup.Id == groupId && s.Id == id)
                    .ToAsyncEnumerable()
                    .ToList();
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
