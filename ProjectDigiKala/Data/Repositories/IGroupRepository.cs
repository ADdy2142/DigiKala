using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IGroupRepository
    {
        void Add(Group group);
        Task AddAsync(Group group);

        void Update(Group group);
        Task UpdateAsync(Group group);

        void Delete(Group group);
        Task DeleteAsync(Group group);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Group GetGroupById(int id);
        Task<Group> GetGroupByIdAsync(int id);

        IEnumerable<Group> GetGroups(int? id, byte? state, string title = "", string slug = "");
        Task<IEnumerable<Group>> GetGroupsAsync(int? id, byte? state, string title = "", string slug = "");

        void Save();
        Task SaveAsync();
    }
}
