using ProjectDigiKala.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ITagValueRepository
    {
        void Add(TagValue tagValue);
        Task AddAsync(TagValue tagValue);

        void Update(TagValue tagValue);
        Task UpdateAsync(TagValue tagValue);

        void Delete(TagValue tagValue);
        Task DeleteAsync(TagValue tagValue);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        TagValue GetTagValueById(int id);
        Task<TagValue> GetTagValueByIdAsync(int id);

        IEnumerable<TagValue> GetTagValues(int? tagId, int id = 0, string title = "", State state = State.Enable);
        Task<IEnumerable<TagValue>> GetTagValuesAsync(int? tagId, int id = 0, string title = "", State state = State.Enable);

        void Save();
        Task SaveAsync();
    }
}
