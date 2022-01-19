using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Carts;

namespace ProjectDigiKala.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private UserManager<Operator> _userManager;
        private ICartRepository _cartRepository;
        private IProductItemRepository _productItemRepository;

        public CartController(UserManager<Operator> userManager, ICartRepository cartRepository, IProductItemRepository productItemRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productItemRepository = productItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? productItemId, int? count)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account");
            else
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userClaims = await _userManager.GetClaimsAsync(user);
                if (userClaims.Any(c => c.Value == "Customer"))
                {
                    var cart = await _cartRepository.GetCartByCustomerIdAsync(user.Id);

                    if (productItemId == null && count == null)
                    {
                        if (cart != null)
                            return View(cart);
                        else
                            return new NotFoundResult();
                    }
                    else
                    {
                        if (productItemId == null || count == null)
                            return new BadRequestResult();

                        var productItem = await _productItemRepository.GetProductItemByIdAsync(productItemId.Value);
                        if (productItem == null)
                            return new NotFoundResult();

                        if (cart == null)
                        {
                            var shoppingCart = new Cart()
                            {
                                Customer = user
                            };

                            await _cartRepository.AddAsync(shoppingCart);
                            await _cartRepository.SaveAsync();

                            var shoppingCartFinal = await _cartRepository.GetCartByIdAsync(shoppingCart.Id);

                            await _cartRepository.AddAsync(new CartItem()
                            {
                                Cart = shoppingCartFinal,
                                ProductItem = productItem,
                                Quantity = count.Value
                            });

                            await _cartRepository.SaveAsync();
                        }
                        else
                        {
                            var cartItem = await _cartRepository.GetCartItemByProductItemIdAsync(productItem.Id, cart.Id);
                            if (cartItem == null)
                            {
                                await _cartRepository.AddAsync(new CartItem()
                                {
                                    Cart = cart,
                                    ProductItem = productItem,
                                    Quantity = count.Value
                                });
                                await _cartRepository.SaveAsync();
                            }
                            else
                            {
                                cartItem.Quantity += count.Value;
                                await _cartRepository.UpdateAsync(cartItem);
                                await _cartRepository.SaveAsync();
                            }
                        }

                        cart = await _cartRepository.GetCartByCustomerIdAsync(user.Id);
                        return View(cart);
                    }
                }
                else
                    return RedirectToAction("SignIn", "Account");
            }
        }

        [HttpGet]
        [Route("[controller]/[action]/{cartItemId:int?}")]
        public async Task<IActionResult> Remove(int? cartItemId)
        {
            if (cartItemId == null)
                return new BadRequestResult();

            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId.Value);
            if (cartItem == null)
                return new NotFoundResult();

            await _cartRepository.DeleteAsync(cartItem);
            await _cartRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
