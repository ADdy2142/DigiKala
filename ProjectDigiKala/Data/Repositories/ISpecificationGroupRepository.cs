using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ISpecificationGroupRepository
    {
        void Add(SpecificationGroup specificationGroup);
        Task AddAsync(SpecificationGroup specificationGroup);

        void Update(SpecificationGroup specificationGroup);
        Task UpdateAsync(SpecificationGroup specificationGroup);

        void Delete(SpecificationGroup specificationGroup);
        Task DeleteAsync(SpecificationGroup specificationGroup);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        SpecificationGroup GetSpecificationGroupById(int id);
        Task<SpecificationGroup> GetSpecificationGroupByIdAsync(int id);

        IEnumerable<SpecificationGroup> GetSpecificationGroups(int groupId, int id = 0, string title = "", State state = State.Enable);
        Task<IEnumerable<SpecificationGroup>> GetSpecificationGroupsAsync(int groupId, int id = 0, string title = "", State state = State.Enable);

        void Save();
        Task SaveAsync();
    }
}
