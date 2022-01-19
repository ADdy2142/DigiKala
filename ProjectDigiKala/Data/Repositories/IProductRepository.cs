using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        Task AddAsync(Product product);

        void Update(Product product);
        Task UpdateAsync(Product product);

        void Delete(Product product);
        Task DeleteAsync(Product product);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Product GetProductById(int id);
        Task<Product> GetProductByIdAsync(int id);

        IEnumerable<Product> GetProducts(int? productId, int? brandId, int? groupId, byte? state, string title = "");
        Task<IEnumerable<Product>> GetProductsAsync(int? productId, int? brandId, int? groupId, byte? state, string title = "");
        IEnumerable<Product> GetProducts(string keyword, double? fromPrice, double? toPrice, int[] brands, int[] specifications);
        Task<IEnumerable<Product>> GetProductsAsync(string keyword, double? fromPrice, double? toPrice, int[] brands, int[] specifications);
        IEnumerable<Product> GetProducts(int groupId);
        Task<IEnumerable<Product>> GetProductsAsync(int groupId);
        Product GetProductDetails(int productId);
        Task<Product> GetProductDetailsAsync(int productId);

        void Save();
        Task SaveAsync();
    }
}
