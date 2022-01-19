using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Carts;
using ProjectDigiKala.Models.Orders;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.ViewModels;

namespace ProjectDigiKala.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private UserManager<Operator> _userManager;
        private ICartRepository _cartRepository;
        private IAddressRepository _addressRepository;
        private IOrderRepository _orderRepository;
        private IProductItemRepository _productItemRepository;

        public OrderController(UserManager<Operator> userManager, ICartRepository cartRepository, IAddressRepository addressRepository, IOrderRepository orderRepository, IProductItemRepository productItemRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _productItemRepository = productItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cart = await _cartRepository.GetCartByCustomerIdAsync(user.Id);
            var totalPrice = cart.CartItems.Sum(c =>
                c.Quantity * (c.ProductItem.Price - ((c.ProductItem.Discount * c.ProductItem.Price) / 100))).ToString("N0");

            var addresses = await _addressRepository.GetAddressesAsync(user.Id);

            var model = new OrderIndexViewModel()
            {
                Addresses = addresses,
                TotalPrice = totalPrice
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? address, byte? shippingType, byte? paymentType)
        {
            if (address == null || shippingType == null || paymentType == null)
                return new BadRequestResult();

            var selectedAddress = await _addressRepository.GetAddressByIdAsync(address.Value);
            if (selectedAddress == null)
                return new NotFoundResult();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var cart = await _cartRepository.GetCartByCustomerIdAsync(user.Id);
                if (cart != null)
                {
                    var totalPrice = cart.CartItems.Sum(c =>
                        c.Quantity * (c.ProductItem.Price - ((c.ProductItem.Discount * c.ProductItem.Price) / 100)));

                    switch (shippingType)
                    {
                        case 1:
                            totalPrice += 15000;
                            break;
                        case 2:
                            totalPrice += 12000;
                            break;
                    }

                    var order = new Order()
                    {
                        Address = selectedAddress,
                        Customer = user,
                        ShippingType = (ShippingType)shippingType,
                        PaymentType = (PaymentType)paymentType,
                        TotalPrice = totalPrice,
                        OrderDate = DateTime.Now,
                        PaymentState = PaymentState.Unpaid
                    };

                    await _orderRepository.AddAsync(order);
                    await _cartRepository.SaveAsync();

                    var orderItems = new List<OrderItem>();
                    foreach (var cartItem in cart.CartItems)
                    {
                        orderItems.Add(new OrderItem()
                        {
                            ProductItem = cartItem.ProductItem,
                            Order = order,
                            Price = (cartItem.ProductItem.Price - ((cartItem.ProductItem.Price * cartItem.ProductItem.Discount) / 100)),
                            Quantity = cartItem.Quantity
                        });
                    }

                    await _orderRepository.AddRangeAsync(orderItems);
                    await _orderRepository.SaveAsync();

                    List<ProductItem> productItems = new List<ProductItem>();
                    foreach (var cartItem in cart.CartItems)
                    {
                        var productItem = cartItem.ProductItem;
                        if (!productItems.Contains(productItem))
                        {
                            productItem.Quantity = (byte)(productItem.Quantity - cartItem.Quantity);
                            productItems.Add(productItem);
                        }
                    }

                    await _productItemRepository.UpdateRangeAsync(productItems);
                    await _productItemRepository.SaveAsync();

                    await _cartRepository.DeleteRangeAsync(cart.CartItems);
                    await _cartRepository.DeleteAsync(cart);
                    await _cartRepository.SaveAsync();

                    return RedirectToAction(nameof(Detail), new { id = order.Id });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = await _orderRepository.GetOrderByIdAsync(id.Value, user.Id);
            if (order == null)
                return new NotFoundResult();

            return View(order);
        }
    }
}
