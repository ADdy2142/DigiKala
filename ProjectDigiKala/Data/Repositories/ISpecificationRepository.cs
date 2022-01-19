using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ISpecificationRepository
    {
        void Add(Specification specification);
        Task AddAsync(Specification specification);

        void Update(Specification specification);
        Task UpdateAsync(Specification specification);

        void Delete(Specification specification);
        Task DeleteAsync(Specification specification);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Specification GetSpecificationById(int id);
        Task<Specification> GetSpecificationByIdAsync(int id);

        IEnumerable<Specification> GetSpecifications(int groupId, int id, string title, State state);
        Task<IEnumerable<Specification>> GetSpecificationsAsync(int groupId, int id, string title, State state);

        void Save();
        Task SaveAsync();
    }
}
