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
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public CartController(
            UserManager<User> userManager,
            ICartService service,
            IOrderService orderService)
        {
            _service = service;
            _orderService = orderService;            
            _userManager = userManager;
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
            ViewBag.CustomersId = userId;

            return View(cartViewModel);
        }

        public async Task<IActionResult> AddToCart(CartDetails cartDetails)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartItems = await _service.GetCartItemsByProductId(cartDetails.ProductId, userId);

            if (cartItems == null)
                _service.AddToCartItems(cartDetails);
            else {
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

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromCart(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var _cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (_cartDetails == null) return RedirectToAction("Error", "Home");

            ViewBag.CartCount = await _service.GetCartTotalQty(userId);

            return View();
        }

        [HttpPost, ActionName("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCartConfirmed(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cartDetails = await _service.GetCartItemsByProductId(id, userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            ViewBag.CartCount = await _service.GetCartTotalQty(userId);

            await _service.RemoveFromCart(id, userId);

            return RedirectToAction(nameof(Index));
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
                ViewBag.CustomersId = userId;

                // Update Cart
                await _service.UpdateCart(userId, cart);

                return View(cartViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrderConfirmed(Cart cart)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var cartViewModel = new CartViewModel();
            var cartDetails = await _service.GetCartItems(userId);

            if (cart.CustomersId != null)
            {
                cartViewModel.Cart = cart;
                cartViewModel.CartDetails = cartDetails;
                ViewBag.CartCount = await _service.GetCartTotalQty(userId);
                ViewBag.CustomersId = userId;
                var user = await _userManager.FindByIdAsync(userId);
                ViewBag.Customer = user.FirstName;

                //if (cart.IsPayMaya)
                //{
                //    return RedirectToAction("PaymentRedirect", cartViewModel);
                //}

                // Update Cart
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
                OrderDetails = orderDetails
            };

            var result = _orderService.AddToOrder(order);

            if (result)
                await _service.EmptyCart(userId); //temporary only

            return View(cartViewModel);
        }

        public async Task<IActionResult> PaymentRedirect(CartViewModel cart)
        {
            return View(cart);
        }
    }
}
