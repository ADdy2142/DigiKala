using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Carts;

namespace ProjectDigiKala.Data.Services
{
    public class CartRepository : ICartRepository
    {
        private ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) => _context = context;

        public void Add(Cart cart) => _context
            .Carts
            .Add(cart);

        public async Task AddAsync(Cart cart) => await _context
            .Carts
            .AddAsync(cart);

        public void Add(CartItem cartItem) => _context
            .CartItems
            .Add(cartItem);

        public async Task AddAsync(CartItem cartItem) => await _context
            .CartItems
            .AddAsync(cartItem);

        public void Update(CartItem cartItem)
        {
            var mainCartItem = GetCartItemById(cartItem.Id);
            mainCartItem.Quantity = cartItem.Quantity;
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            var mainCartItem = await GetCartItemByIdAsync(cartItem.Id);
            mainCartItem.Quantity = cartItem.Quantity;
        }

        public void Delete(Cart cart) => _context
            .Carts
            .Remove(cart);

        public async Task DeleteAsync(Cart cart) => await Task.Run(() =>
        {
            Delete(cart);
        });

        public void Delete(CartItem cartItem) => _context
            .CartItems
            .Remove(cartItem);

        public async Task DeleteAsync(CartItem cartItem) => await Task.Run(() =>
        {
            Delete(cartItem);
        });

        public void DeleteCartById(int id)
        {
            var cart = GetCartById(id);
            Delete(cart);
        }

        public async Task DeleteCartByIdAsync(int id)
        {
            var cart = await GetCartByIdAsync(id);
            await DeleteAsync(cart);
        }

        public void DeleteCartItemById(int id)
        {
            var cartItem = GetCartItemById(id);
            Delete(cartItem);
        }

        public async Task DeleteCartItemByIdAsync(int id)
        {
            var cartItem = await GetCartItemByIdAsync(id);
            await DeleteAsync(cartItem);
        }

        public void DeleteRange(IEnumerable<CartItem> cartItems) => _context
            .CartItems
            .RemoveRange(cartItems);

        public async Task DeleteRangeAsync(IEnumerable<CartItem> cartItems) => await Task.Run(() =>
        {
            DeleteRange(cartItems);
        });

        public Cart GetCartById(int id) => _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .ThenInclude(c => c.ProductItem)
            .ThenInclude(p => p.Product)
            .SingleOrDefault(c => c.Id == id);

        public async Task<Cart> GetCartByIdAsync(int id) => await _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .ThenInclude(c => c.ProductItem)
            .ThenInclude(p => p.Product)
            .SingleOrDefaultAsync(c => c.Id == id);

        public Cart GetCartByCustomerId(string customerId) => _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .ThenInclude(c => c.ProductItem)
            .ThenInclude(p => p.Product)
            .SingleOrDefault(c => c.Customer.Id == customerId);

        public async Task<Cart> GetCartByCustomerIdAsync(string customerId) => await _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .ThenInclude(c => c.ProductItem)
            .ThenInclude(p => p.Product)
            .SingleOrDefaultAsync(c => c.Customer.Id == customerId);

        public CartItem GetCartItemById(int id) => _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem)
            .SingleOrDefault(c => c.Id == id);

        public async Task<CartItem> GetCartItemByIdAsync(int id) => await _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem)
            .SingleOrDefaultAsync(c => c.Id == id);

        public CartItem GetCartItemByProductItemId(int productItemId, int cartId) => _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem)
            .SingleOrDefault(c => c.ProductItem.Id == productItemId && c.Cart.Id == cartId);

        public async Task<CartItem> GetCartItemByProductItemIdAsync(int productItemId, int cartId) => await _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem)
            .SingleOrDefaultAsync(c => c.ProductItem.Id == productItemId && c.Cart.Id == cartId);

        public IEnumerable<Cart> GetCarts(string customerId) => _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .Where(c => c.Customer.Id == customerId);

        public async Task<IEnumerable<Cart>> GetCartsAsync(string customerId) => await _context
            .Carts
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
            .Where(c => c.Customer.Id == customerId)
            .ToListAsync();

        public IEnumerable<CartItem> GetCartItems() => _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem);

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync() => await _context
            .CartItems
            .Include(c => c.Cart)
            .Include(c => c.ProductItem)
            .ToListAsync();

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
