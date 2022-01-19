using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Products;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IProductItemRepository
    {
        void Add(ProductItem productItem);
        Task AddAsync(ProductItem productItem);
        void AddRange(IEnumerable<ProductItem> productItems);
        Task AddRangeAsync(IEnumerable<ProductItem> productItems);
        void AddProductItemTagValues(IEnumerable<ProductItemTagValue> productItemTagValues);
        Task AddProductItemTagValuesAsync(IEnumerable<ProductItemTagValue> productItemTagValues);

        void Update(ProductItem productItem);
        Task UpdateAsync(ProductItem productItem);
        void UpdateRange(IEnumerable<ProductItem> productItems);
        Task UpdateRangeAsync(IEnumerable<ProductItem> productItems);

        void Delete(ProductItem productItem);
        Task DeleteAsync(ProductItem productItem);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);
        void DeleteProductItemTagValues(IEnumerable<ProductItemTagValue> productItemTagValues);
        Task DeleteProductItemTagValuesAsync(IEnumerable<ProductItemTagValue> productItemTagValues);

        ProductItem GetProductItemById(int id);
        Task<ProductItem> GetProductItemByIdAsync(int id);

        IEnumerable<ProductItem> GetProductItems(int id = 0, int productId = 0, State state = State.Enable);
        Task<IEnumerable<ProductItem>> GetProductItemsAsync(int id = 0, int productId = 0, State state = State.Enable);

        void Save();
        Task SaveAsync();
    }
}
