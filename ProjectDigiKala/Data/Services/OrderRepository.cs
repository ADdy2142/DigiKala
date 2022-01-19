using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Orders;

namespace ProjectDigiKala.Data.Services
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) => _context = context;

        public void Add(Order order) => _context
            .Orders
            .Add(order);

        public async Task AddAsync(Order order) => await _context
            .Orders
            .AddAsync(order);

        public void AddRange(IEnumerable<OrderItem> orderItems) => _context
            .OrderItems
            .AddRange(orderItems);

        public async Task AddRangeAsync(IEnumerable<OrderItem> orderItems) => await _context
            .OrderItems
            .AddRangeAsync(orderItems);

        public Order GetOrderById(int id, string customerId) => _context
            .Orders
            .Include(o => o.Customer)
            .ThenInclude(o => o.Addresses)
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.ProductItem)
            .ThenInclude(p => p.Product)
            .Include(o => o.Address)
            .SingleOrDefault(o => o.Id == id && o.Customer.Id == customerId);

        public async Task<Order> GetOrderByIdAsync(int id, string customerId) => await _context
            .Orders
            .Include(o => o.Customer)
            .ThenInclude(o => o.Addresses)
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.ProductItem)
            .ThenInclude(p => p.Product)
            .Include(o => o.Address)
            .SingleOrDefaultAsync(o => o.Id == id && o.Customer.Id == customerId);

        public void Save() => _context
            .SaveChanges();

        public async Task SaveAsync() => await _context
            .SaveChangesAsync();
    }
}
