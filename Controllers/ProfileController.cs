﻿using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ProfileController : Controller
    {
        private readonly IProfileService _service;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public ProfileController(IProfileService service
            , ICartService cartService
            , IOrderService orderService
            , UserManager<User> userManager)
        {
            _service = service;
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _userManager.FindByIdAsync(userId);

            return View(result);
        }

        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _service.GetCustomerOrders(userId);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Customer = user;

            return View(result);
        }

        public async Task<IActionResult> Returns()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _service.GetCustomerReturns(userId);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Customer = user;

            return View(result);
        }

        public async Task<IActionResult> Wishlist()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);

            var result = await _service.GetCustomerWishlist(userId);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Customer = user;

            return View(result);
        }

        [Route("Profile/ViewOrder/{transactionId:Guid}")]
        public async Task<IActionResult> ViewOrder(Guid transactionId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var user = await _userManager.FindByIdAsync(userId);
            var order = _orderService.GetOrderById(transactionId.ToString());
            var orderDetails = await _orderService.GetOrderDetailsById(transactionId.ToString());

            var viewModel = new OrderViewModel();
            viewModel.CustomersId = userId;
            viewModel.Customer = user.FirstName;
            viewModel.Address = user.AddressCity;
            viewModel.TransactionId = transactionId;
            viewModel.DeliveryStatusId = order.DeliveryStatusId;
            viewModel.OrderStatusId = order.OrderStatusId;
            viewModel.OrderDetails = orderDetails;
            viewModel.OrderStatus = order.OrderStatus;

            ViewBag.Customer = user;
            return View(viewModel);
        }

        public async Task<IActionResult> Addresses()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _service.GetCustomerBillingAddresses(userId);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Customer = user;

            return View(result);
        }

        [Route("Profile/ReturnOrder/{productId:long}")]
        public async Task<IActionResult> ReturnOrder(long productId, OrderViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var user = await _userManager.FindByIdAsync(userId);
            viewModel.Customer = user.FirstName;
            viewModel.Address = user.AddressCity;
            viewModel.OrderDetails = await _orderService.GetOrderDetailsById(viewModel.TransactionId.ToString());
            viewModel.ProductId = productId;

            ViewBag.Customer = user;

            return View(viewModel);
        }
        public IActionResult ReturnSuccess()
        {
            return View();
        }

    }
}
