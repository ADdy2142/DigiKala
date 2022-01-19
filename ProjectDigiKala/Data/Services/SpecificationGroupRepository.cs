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
    public class SpecificationGroupRepository : ISpecificationGroupRepository
    {
        private ApplicationDbContext _context;

        public SpecificationGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SpecificationGroup specificationGroup) => _context.Add(specificationGroup);

        public async Task AddAsync(SpecificationGroup specificationGroup) =>
            await _context.AddAsync(specificationGroup);

        public void Update(SpecificationGroup specificationGroup)
        {
            var mainSpecificationGroup = GetSpecificationGroupById(specificationGroup.Id);
            mainSpecificationGroup.State = specificationGroup.State;
            mainSpecificationGroup.LastModifier = specificationGroup.LastModifier;
            mainSpecificationGroup.LastModifyDate = specificationGroup.LastModifyDate;
            mainSpecificationGroup.Title = specificationGroup.Title;
        }

        public async Task UpdateAsync(SpecificationGroup specificationGroup)
        {
            var mainSpecificationGroup = await GetSpecificationGroupByIdAsync(specificationGroup.Id);
            mainSpecificationGroup.State = specificationGroup.State;
            mainSpecificationGroup.LastModifier = specificationGroup.LastModifier;
            mainSpecificationGroup.LastModifyDate = specificationGroup.LastModifyDate;
            mainSpecificationGroup.Title = specificationGroup.Title;
        }

        public void Delete(SpecificationGroup specificationGroup) => _context.Remove(specificationGroup);

        public async Task DeleteAsync(SpecificationGroup specificationGroup)
        {
            await Task.Run(() =>
            {
                _context.Remove(specificationGroup);
            });
        }

        public void DeleteById(int id)
        {
            var specificationGroup = GetSpecificationGroupById(id);
            Delete(specificationGroup);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var specificationGroup = await GetSpecificationGroupByIdAsync(id);
            await DeleteAsync(specificationGroup);
        }

        public SpecificationGroup GetSpecificationGroupById(int id) => _context
            .SpecificationGroups
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.Group)
            .Include(s => s.Specifications)
            .SingleOrDefault(s => s.Id == id);

        public async Task<SpecificationGroup> GetSpecificationGroupByIdAsync(int id) => await _context
            .SpecificationGroups
            .Include(s => s.Creator)
            .Include(s => s.LastModifier)
            .Include(s => s.Group)
            .Include(s => s.Specifications)
            .SingleOrDefaultAsync(s => s.Id == id);

        public IEnumerable<SpecificationGroup> GetSpecificationGroups(int groupId, int id = 0, string title = "", State state = State.Enable)
        {
            if (id == 0)
                return _context
                    .SpecificationGroups
                    .Include(s => s.Creator)
                    .Include(s => s.Group)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Specifications)
                    .Where(s => s.Group.Id == groupId && (s.Title.Contains(title) || s.State == state))
                    .ToList();
            else
                return _context
                    .SpecificationGroups
                    .Include(s => s.Creator)
                    .Include(s => s.Group)
                    .Include(s => s.LastModifier)
                    .Include(s => s.Specifications)
                    .Where(s => s.Group.Id == groupId && s.Id == id)
                    .ToList();
        }

        public async Task<IEnumerable<SpecificationGroup>> GetSpecificationGroupsAsync(int groupId, int id = 0, string title = "", State state = State.Enable)
        {
            var list = await _context
                .SpecificationGroups
                .Include(s => s.Creator)
                .Include(s => s.Group)
                .Include(s => s.LastModifier)
                .Include(s => s.Specifications)
                .ThenInclude(s => s.SpecificationValue)
                .ToAsyncEnumerable()
                .ToList();

            if (id == 0)
                return list.Where(s => s.Group.Id == groupId && (s.Title.Contains(title) || s.State == state));
            else
                return list.Where(s => s.Group.Id == groupId && s.Id == id);
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
