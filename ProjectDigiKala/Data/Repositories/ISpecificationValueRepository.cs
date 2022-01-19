using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ISpecificationValueRepository
    {
        void Add(SpecificationValue specificationValue);
        Task AddAsync(SpecificationValue specificationValue);
        Task AddRangeAsync(IEnumerable<SpecificationValue> specificationValues);

        void Update(SpecificationValue specificationValue);
        Task UpdateAsync(SpecificationValue specificationValue);
        Task UpdateRangeAsync(IEnumerable<SpecificationValue> specificationValues);

        void Delete(SpecificationValue specificationValue);
        Task DeleteAsync(SpecificationValue specificationValue);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);
        void DeleteRange(IEnumerable<SpecificationValue> specificationValues);
        Task DeleteRangeAsync(IEnumerable<SpecificationValue> specificationValues);

        SpecificationValue GetSpecificationValueById(int id);
        Task<SpecificationValue> GetSpecificationValueByIdAsync(int id);

        IEnumerable<SpecificationValue> GetSpecificationValues(int id = 0, string title = "", State state = State.Enable);
        Task<IEnumerable<SpecificationValue>> GetSpecificationValuesAsync(int id = 0, string title = "", State state = State.Enable);
        IEnumerable<SpecificationValue> GetSpecificationValues(int productId);
        Task<IEnumerable<SpecificationValue>> GetSpecificationValuesAsync(int productId);

        void Save();
        Task SaveAsync();
    }
}
