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
    public class GroupRepository : IGroupRepository
    {
        private ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context) => _context = context;

        public void Add(Group group) => _context.Groups.Add(group);

        public async Task AddAsync(Group group) => await _context.Groups.AddAsync(group);

        public void Delete(Group group) => _context.Groups.Remove(group);

        public async Task DeleteAsync(Group group)
        {
            await Task.Run(() =>
            {
                Delete(group);
            });
        }

        public void DeleteById(int id)
        {
            var group = GetGroupById(id);
            Delete(group);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var group = await GetGroupByIdAsync(id);
            await DeleteAsync(group);
        }

        public Group GetGroupById(int id) => _context.Groups.SingleOrDefault(g => g.Id == id);

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                return GetGroupById(id);
            });
        }

        /*
         * .Include(p => p.Products).Include(o => o.Creator).Include(o => o.LastModifier)
         */

        public IEnumerable<Group> GetGroups(int? id, byte? state, string title = "", string slug = "")
        {
            List<Group> groups;
            if (id != null)
                groups = _context.Groups.Where(b => b.Id == id).Include(b => b.Creator).Include(b => b.LastModifier).ToList();
            else
                groups = _context.Groups.Where(b => b.Title.Contains(title) || b.Slug.Contains(slug)).Include(b => b.Creator).Include(b => b.LastModifier).ToList();

            if (state != null)
                groups = groups.Where(b => b.State == (State)state).ToList();

            return groups;
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync(int? id, byte? state, string title = "", string slug = "")
        {
            List<Group> groups;
            if (id != null)
                groups = await _context.Groups.Where(b => b.Id == id).Include(b => b.Creator).Include(b => b.LastModifier).ToAsyncEnumerable().ToList();
            else
                groups = await _context.Groups.Where(b => b.Title.Contains(title) || b.Slug.Contains(slug)).Include(b => b.Creator).Include(b => b.LastModifier).ToAsyncEnumerable().ToList();

            if (state != null)
                groups = await groups.Where(b => b.State == (State)state).ToAsyncEnumerable().ToList();

            return groups;
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(Group group)
        {
            var mainGroup = GetGroupById(group.Id);
            mainGroup.LastModifier = group.LastModifier;
            mainGroup.LastModifyDate = group.LastModifyDate;
            mainGroup.Slug = group.Slug;
            mainGroup.Title = group.Title;
            mainGroup.State = group.State;
        }

        public async Task UpdateAsync(Group group)
        {
            await Task.Run(() =>
            {
                Update(group);
            });
        }
    }
}
