using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IBrandRepository
    {
        void Add(Brand brand);
        Task AddAsync(Brand brand);

        void Update(Brand brand);
        Task UpdateAsync(Brand brand);

        void Delete(Brand brand);
        Task DeleteAsync(Brand brand);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Brand GetBrandById(int id);
        Task<Brand> GetBrandByIdAsync(int id);

        IEnumerable<Brand> GetBrands(int? id, byte? state, string title = "", string slug = "");
        Task<IEnumerable<Brand>> GetBrandsAsync(int? id, byte? state, string title = "", string slug = "");

        void Save();
        Task SaveAsync();
    }
}
