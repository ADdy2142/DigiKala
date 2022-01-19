using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Carts;

namespace ProjectDigiKala.Data.Repositories
{
    public interface ICartRepository
    {
        void Add(Cart cart);
        Task AddAsync(Cart cart);
        void Add(CartItem cartItem);
        Task AddAsync(CartItem cartItem);

        void Update(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);

        void Delete(Cart cart);
        Task DeleteAsync(Cart cart);
        void Delete(CartItem cartItem);
        Task DeleteAsync(CartItem cartItem);
        void DeleteCartById(int id);
        Task DeleteCartByIdAsync(int id);
        void DeleteCartItemById(int id);
        Task DeleteCartItemByIdAsync(int id);
        void DeleteRange(IEnumerable<CartItem> cartItems);
        Task DeleteRangeAsync(IEnumerable<CartItem> cartItems);

        Cart GetCartById(int id);
        Task<Cart> GetCartByIdAsync(int id);
        Cart GetCartByCustomerId(string customerId);
        Task<Cart> GetCartByCustomerIdAsync(string customerId);
        CartItem GetCartItemById(int id);
        Task<CartItem> GetCartItemByIdAsync(int id);
        CartItem GetCartItemByProductItemId(int productItemId, int cartId);
        Task<CartItem> GetCartItemByProductItemIdAsync(int productItemId, int cartId);
        IEnumerable<Cart> GetCarts(string customerId);
        Task<IEnumerable<Cart>> GetCartsAsync(string customerId);
        IEnumerable<CartItem> GetCartItems();
        Task<IEnumerable<CartItem>> GetCartItemsAsync();

        void Save();
        Task SaveAsync();
    }
}
