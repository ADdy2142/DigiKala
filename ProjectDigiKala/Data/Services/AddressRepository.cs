using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Profile;

namespace ProjectDigiKala.Data.Services
{
    public class AddressRepository : IAddressRepository
    {
        private ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context) => _context = context;

        public void Add(Address address) => _context
            .Addresses
            .Add(address);

        public async Task AddAsync(Address address) => await _context
            .Addresses
            .AddAsync(address);

        public void Update(Address address)
        {
            var mainAddress = GetAddressById(address.Id);
            mainAddress.City = address.City;
            mainAddress.Location = address.Location;
            mainAddress.Phone = address.Phone;
            mainAddress.Province = address.Province;
        }

        public async Task UpdateAsync(Address address)
        {
            var mainAddress = await GetAddressByIdAsync(address.Id);
            mainAddress.City = address.City;
            mainAddress.Location = address.Location;
            mainAddress.Phone = address.Phone;
            mainAddress.Province = address.Province;
        }

        public void Delete(Address address) => _context
            .Addresses
            .Remove(address);

        public async Task DeleteAsync(Address address) => await Task.Run(() =>
        {
            Delete(address);
        });

        public void DeleteById(int id)
        {
            var address = GetAddressById(id);
            Delete(address);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var address = await GetAddressByIdAsync(id);
            await DeleteAsync(address);
        }

        public Address GetAddressById(int id) => _context
            .Addresses
            .Include(a => a.Customer)
            .SingleOrDefault(a => a.Id == id);

        public async Task<Address> GetAddressByIdAsync(int id) => await _context
            .Addresses
            .Include(a => a.Customer)
            .SingleOrDefaultAsync(a => a.Id == id);

        public IEnumerable<Address> GetAddresses(string customerId) => _context
            .Addresses
            .Include(a => a.Customer)
            .Where(a => a.Customer.Id == customerId);

        public async Task<IEnumerable<Address>> GetAddressesAsync(string customerId) => await _context
            .Addresses
            .Include(a => a.Customer)
            .Where(a => a.Customer.Id == customerId)
            .ToListAsync();

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
