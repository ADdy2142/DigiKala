using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Orders;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task AddAsync(Order order);
        void AddRange(IEnumerable<OrderItem> orderItems);
        Task AddRangeAsync(IEnumerable<OrderItem> orderItems);

        Order GetOrderById(int id, string customerId);
        Task<Order> GetOrderByIdAsync(int id, string customerId);

        void Save();
        Task SaveAsync();
    }
}
