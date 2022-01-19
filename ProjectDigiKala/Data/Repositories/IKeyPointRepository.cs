using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IKeyPointRepository
    {
        void Add(KeyPoint keyPoint);
        Task AddAsync(KeyPoint keyPoint);

        void Update(KeyPoint keyPoint);
        Task UpdateAsync(KeyPoint keyPoint);

        void Delete(KeyPoint keyPoint);
        Task DeleteAsync(KeyPoint keyPoint);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        KeyPoint GetKeyPointById(int id);
        Task<KeyPoint> GetKeyPointByIdAsync(int id);

        IEnumerable<KeyPoint> GetKeyPoints(int productId);
        Task<IEnumerable<KeyPoint>> GetKeyPointsAsync(int productId);

        void Save();
        Task SaveAsync();

        bool IsBelongToProduct(Product product, KeyPoint keyPoint);
        Task<bool> IsBelongToProductAsync(Product product, KeyPoint keyPoint);
    }
}
