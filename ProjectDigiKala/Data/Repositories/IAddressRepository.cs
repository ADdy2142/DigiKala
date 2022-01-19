using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Profile;

namespace ProjectDigiKala.Data.Repositories
{
    public interface IAddressRepository
    {
        void Add(Address address);
        Task AddAsync(Address address);

        void Update(Address address);
        Task UpdateAsync(Address address);

        void Delete(Address address);
        Task DeleteAsync(Address address);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);

        Address GetAddressById(int id);
        Task<Address> GetAddressByIdAsync(int id);
        IEnumerable<Address> GetAddresses(string customerId);
        Task<IEnumerable<Address>> GetAddressesAsync(string customerId);

        void Save();
        Task SaveAsync();
    }
}
