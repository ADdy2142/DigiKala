using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Services
{
    public class TagValueRepository : ITagValueRepository
    {
        private ApplicationDbContext _context;

        public TagValueRepository(ApplicationDbContext context) => _context = context;

        public void Add(TagValue tagValue) => _context.TagValues.Add(tagValue);

        public async Task AddAsync(TagValue tagValue) => await _context.TagValues.AddAsync(tagValue);

        public void Delete(TagValue tagValue) => _context.TagValues.Remove(tagValue);

        public async Task DeleteAsync(TagValue tagValue)
        {
            await Task.Run(new Action(() =>
            {
                Delete(tagValue);
            }));
        }

        public void DeleteById(int id)
        {
            var tagValue = GetTagValueById(id);
            Delete(tagValue);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var tagValue = await GetTagValueByIdAsync(id);
            await DeleteAsync(tagValue);
        }

        public TagValue GetTagValueById(int id)
        {
            return _context.TagValues
                .Include(t => t.Creator)
                .Include(t => t.LastModifier)
                .Include(t => t.Tag)
                .SingleOrDefault(t => t.Id == id);
        }

        public async Task<TagValue> GetTagValueByIdAsync(int id)
        {
            return await _context.TagValues
                .Include(t => t.Creator)
                .Include(t => t.LastModifier)
                .Include(t => t.Tag)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<TagValue> GetTagValues(int? tagId, int id = 0, string title = "", State state = State.Enable)
        {
            if (id == 0)
                return _context.TagValues
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.Tag)
                    .Where(t => t.Tag.Id == tagId.Value && (t.Title.Contains(title) || t.State == state));
            else
                return _context.TagValues
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.Tag)
                    .Where(t => t.Tag.Id == tagId.Value && t.Id == id);
        }

        public async Task<IEnumerable<TagValue>> GetTagValuesAsync(int? tagId, int id = 0, string title = "", State state = State.Enable)
        {
            if (id == 0)
                return await _context.TagValues
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.Tag)
                    .Where(t => t.Tag.Id == tagId.Value && (t.Title.Contains(title) || t.State == state))
                    .ToAsyncEnumerable()
                    .ToList();
            else
                return await _context.TagValues
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.Tag)
                    .Where(t => t.Tag.Id == tagId.Value && t.Id == id)
                    .ToAsyncEnumerable()
                    .ToList();
        }

        public void Save() => _context.SaveChanges();

        public Task SaveAsync() => _context.SaveChangesAsync();

        public void Update(TagValue tagValue)
        {
            var mainTagValue = GetTagValueById(tagValue.Id);
            mainTagValue.LastModifier = tagValue.LastModifier;
            mainTagValue.LastModifyDate = tagValue.LastModifyDate;
            mainTagValue.State = tagValue.State;
            mainTagValue.Title = tagValue.Title;
        }

        public async Task UpdateAsync(TagValue tagValue)
        {
            var mainTagValue = await GetTagValueByIdAsync(tagValue.Id);
            mainTagValue.LastModifier = tagValue.LastModifier;
            mainTagValue.LastModifyDate = tagValue.LastModifyDate;
            mainTagValue.State = tagValue.State;
            mainTagValue.Title = tagValue.Title;
        }
    }
}
