using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        Task AddAsync(Tag tag);

        void Update(Tag tag);
        Task UpdateAsync(Tag tag);

        void Delete(Tag tag);
        Task DeleteAsync(Tag tag);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Tag GetTagById(int id);
        Task<Tag> GetTagByIdAsync(int id);

        IEnumerable<Tag> GetTags(int? id, string title, byte? state);
        Task<IEnumerable<Tag>> GetTagsAsync(int? id, string title, byte? state);

        void Save();
        Task SaveAsync();
    }
}
