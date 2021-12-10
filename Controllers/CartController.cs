﻿using Adyen.Model.Checkout;
using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly ICartService _service;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;
        private readonly IAdyenService _adyenService;
        private readonly UserManager<User> _userManager;

        public CartController(
            UserManager<User> userManager
            , ICartService service
            , IEmailService emailService
            , IOrderService orderService
            , IAdyenService adyenService)
        {
            _service = service;
            _emailService = emailService;
            _orderService = orderService;
            _userManager = userManager;
            _adyenService = adyenService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cart = await _service.GetCart(userId);
            var cartDetails = await _service.GetCartItems(userId);
            var cartViewModel = new CartViewModel();

            cartViewModel.Cart = cart;
            cartViewModel.CartDetails = cartDetails;
            ViewBag.CartCount = await _service.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _service.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;

            return View(cartViewModel);
        }

        public async Task<IActionResult> Wishlist()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var wishlists = await _service.GetWishlistItems(userId);
            ViewBag.CartCount = await _service.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _service.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;

            return View(wishlists);
        }

        public async Task<IActionResult> AddToCart(CartDetails cartDetails)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartItems = await _service.GetCartItemsByProductId(cartDetails.ProductId, userId);

            if (cartItems == null)
                _service.AddToCartItems(cartDetails);
            else
            {
                //increment quantity
                cartItems.Quantity += cartDetails.Quantity;
                _service.UpdateCartItems(cartItems);
            }

            // Cart
            var cart = await _service.GetCart(userId);
            if (cart == null)
            {
                var _cart = new Cart();
                _cart.CustomersId = userId;

                await _service.CreateCart(_cart);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddToWishlist(Wishlist wishlist)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var wishlistItems = await _service.GetWishlistItemsByProductId(wishlist.ProductId, userId);

            if (wishlistItems == null)
                _service.AddToWishlist(wishlist);
            else
            {
                await _service.RemoveFromWishlist(wishlist.ProductId, userId);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddToCartByQty(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            //increment quantity
            cartDetails.Quantity += 1;
            _service.UpdateCartItems(cartDetails);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddToCartFromWishlist(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var _wishlistItems = await _service.GetWishlistItemsByProductId(id, userId);
            if (_wishlistItems == null) return RedirectToAction("Error", "Home");

            var cartDetails = new CartDetails();
            cartDetails.CustomersId = _wishlistItems.CustomersId;
            cartDetails.ProductId = _wishlistItems.ProductId;
            cartDetails.Quantity = 1;

            _service.AddToCartItems(cartDetails);

            // Cart
            var cart = await _service.GetCart(userId);
            if (cart == null)
            {
                var _cart = new Cart();
                _cart.CustomersId = userId;

                await _service.CreateCart(_cart);
            }

            await _service.RemoveFromWishlist(id, userId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddToWishlistFromCart(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var _cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (_cartDetails == null) return RedirectToAction("Error", "Home");

            var wishlist = new Wishlist();
            wishlist.CustomersId = _cartDetails.CustomersId;
            wishlist.ProductId = _cartDetails.ProductId;

            _service.AddToWishlist(wishlist);

            await _service.RemoveFromCart(id, userId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveFromCartByQty(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            //decrement quantity
            if (cartDetails.Quantity > 1)
            {
                cartDetails.Quantity -= 1;
                _service.UpdateCartItems(cartDetails);
            }
            else
                await _service.RemoveFromCart(id, userId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveFromCart(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var _cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (_cartDetails == null) return RedirectToAction("Error", "Home");

            ViewBag.CartCount = await _service.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _service.GetWishlistCount(userId);

            return View();
        }

        [HttpPost, ActionName("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCartConfirmed(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            ViewBag.CartCount = await _service.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _service.GetWishlistCount(userId);

            await _service.RemoveFromCart(id, userId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromWishlist(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            ViewBag.CartCount = await _service.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _service.GetWishlistCount(userId);

            await _service.RemoveFromWishlist(id, userId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Checkout(Cart cart)
        {
            if (cart.CustomersId != null)
            {
                var userId = _userManager.GetUserId(HttpContext.User);

                var cartViewModel = new CartViewModel();
                var cartDetails = await _service.GetCartItems(userId);

                cartViewModel.Cart = cart;
                cartViewModel.CartDetails = cartDetails;
                ViewBag.CartCount = await _service.GetCartTotalQty(userId);
                ViewBag.WishlistCount = await _service.GetWishlistCount(userId);
                ViewBag.CustomersId = userId;

                // Update Cart
                await _service.UpdateCart(userId, cart);

                return View(cartViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> OrderConfirmed(Cart cart)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            var cartViewModel = new CartViewModel();
            var cartDetails = await _service.GetCartItems(userId);
            if (cart.CustomersId != null)
            {
                cartViewModel.Cart = cart;
                cartViewModel.CartDetails = cartDetails;
                ViewBag.CartCount = await _service.GetCartTotalQty(userId);
                ViewBag.WishlistCount = await _service.GetWishlistCount(userId);
                ViewBag.CustomersId = userId;
                ViewBag.Customer = user.FirstName;

                await _service.UpdateCart(userId, cart);
            }

            Guid transactionId = Guid.NewGuid();
            List<OrderDetails> orderDetails = new List<OrderDetails>();

            foreach (var item in cartDetails)
            {
                var _orderDetails = new OrderDetails
                {
                    TransactionId = transactionId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SubTotal = (item.Quantity * item.Product.Price),
                };

                orderDetails.Add(_orderDetails);
            }

            var order = new Orders()
            {
                TransactionId = transactionId,
                CustomersId = userId,
                ModeOfPayment = 1,
                OrderDate = DateTime.Now,
                OrderStatusId = (int)OrderStatusEnum.Created,
                Total = cart.Total,
                ShippingFee = cart.ShippingFee,
                TaxAmount = cart.TaxAmount,
                OrderDetails = orderDetails
            };

            var result = _orderService.AddToOrder(order);

            if (result)
                await _service.EmptyCart(userId);

            ViewBag.CartCount = 0;

            // Send E-Receipt
            await _emailService.SendReceipt(user.Email, orderDetails, order);

            if (cart.IsGCash)
            {
                var _resultCheckout = _adyenService.CheckoutUsingGCash(order.TransactionId.ToString()
                    , (long)(order.Total + order.ShippingFee + order.TaxAmount) * 100// Need to multiply into 100 since the adyen automatically convert the last two zeroes into decimal
                    , user);
                Response.Redirect(_resultCheckout.action.url);
            }

            return View(cartViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AdyenPaymentResponse([FromQuery(Name = "redirectResult")] string redirectResult)
        {
            await Task.Delay(0);

            var _result = _adyenService.HandlePaymentResponse(redirectResult);

            ViewBag.IsResult = _result.Item1;
            ViewBag.ResultMessage = _result.Item2;

            return View();
        }
    }
}