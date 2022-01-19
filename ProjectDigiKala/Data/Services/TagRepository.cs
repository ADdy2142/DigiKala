using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Services
{
    public class TagRepository : ITagRepository
    {
        private ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context) => _context = context;

        public void Add(Tag tag) => _context.Add(tag);

        public async Task AddAsync(Tag tag) => await _context.AddAsync(tag);

        public void Delete(Tag tag) => _context.Tags.Remove(tag);

        public async Task DeleteAsync(Tag tag)
        {
            await Task.Run(new Action(() =>
            {
                Delete(tag);
            }));
        }

        public void DeleteById(int id)
        {
            var tag = GetTagById(id);
            Delete(tag);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var tag = await GetTagByIdAsync(id);
            await DeleteAsync(tag);
        }

        public Tag GetTagById(int id) => _context.Tags.Include(t => t.Creator).Include(t => t.LastModifier).Include(t => t.TagValues).SingleOrDefault(t => t.Id == id);

        public async Task<Tag> GetTagByIdAsync(int id) => await _context.Tags.Include(t => t.Creator).Include(t => t.LastModifier).Include(t => t.TagValues).SingleOrDefaultAsync(t => t.Id == id);

        public IEnumerable<Tag> GetTags(int? id, string title, byte? state)
        {
            if (id != null)
                return _context.Tags
                    .Where(t => t.Id == id)
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.TagValues);

            IEnumerable<Tag> result;

            if (state != null)
                result = _context.Tags
                .Where(t => t.Title.Contains(title) && t.State == ((State)state))
                .Include(t => t.Creator)
                .Include(t => t.LastModifier)
                .Include(t => t.TagValues);
            else
                result = _context.Tags
                .Where(t => t.Title.Contains(title))
                .Include(t => t.Creator)
                .Include(t => t.LastModifier)
                .Include(t => t.TagValues);

            return result;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(int? id, string title, byte? state)
        {
            if (id != null)
                return await _context.Tags
                    .Where(t => t.Id == id)
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.TagValues)
                    .ToAsyncEnumerable()
                    .ToList();

            IEnumerable<Tag> result;

            if (state != null)
                result = await _context.Tags
                    .Where(t => t.Title.Contains(title) && t.State == ((State)state))
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.TagValues)
                    .ToAsyncEnumerable()
                    .ToList();
            else
                result = await _context.Tags
                    .Where(t => t.Title.Contains(title))
                    .Include(t => t.Creator)
                    .Include(t => t.LastModifier)
                    .Include(t => t.TagValues)
                    .ToAsyncEnumerable()
                    .ToList();

            return result;
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(Tag tag)
        {
            var mainTag = GetTagById(tag.Id);
            mainTag.LastModifier = tag.LastModifier;
            mainTag.LastModifyDate = tag.LastModifyDate;
            mainTag.State = tag.State;
            mainTag.Title = tag.Title;
        }

        public async Task UpdateAsync(Tag tag)
        {
            var mainTag = await GetTagByIdAsync(tag.Id);
            mainTag.LastModifier = tag.LastModifier;
            mainTag.LastModifyDate = tag.LastModifyDate;
            mainTag.State = tag.State;
            mainTag.Title = tag.Title;
        }
    }
}
