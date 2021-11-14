using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _service;
        private readonly IOrderService _orderService;

        public CartController(ICartService service,
            IOrderService orderService)
        {
            _service = service;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cart);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cart);
        }

        public async Task<IActionResult> DeliveryMethod()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cart);
        }

        public async Task<IActionResult> PaymentMethod()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cart);
        }

        public async Task<IActionResult> OrderReview()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cart);
        }

        public async Task<IActionResult> OrderConfirmed()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            var result = false;
            Guid transactionId = Guid.NewGuid();
            foreach (var item in cart) {

                var order = new OrderDetails()
                {
                    TransactionId = transactionId,
                    ProductId = item.ProductId,
                    ItemQuantity = item.Quantity,
                    ItemSubTotal = (item.Quantity * item.Product.Price),
                    CustomerName = "Test Customer",
                    ModeOfPayment = "Cash",
                    OrderDate = DateTime.Now
                };

               result = _orderService.AddToOrder(order);
            }

            if (result)
                await _service.EmptyCart(); //temporary only

            return View(cart);
        }

        public IActionResult AddToCart(Cart cart)
        {
            //temporary only; will optimize later
            for (int ctr = 0; ctr < cart.Quantity; ctr++)
            {
                var _cart = new Cart()
                {
                    ProductId = cart.ProductId, //change to variant
                    Quantity = 1
                };

                _service.AddToCart(_cart);
            };

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddToCartByQty(long productId)
        {
            var _cart = new Cart()
            {
                ProductId = productId, //change to variant
                Quantity = 1
            };

            _service.AddToCart(_cart);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromCartByQty(long id)
        {
            var cartDetails = await _service.GetCartItemsByProductId(id);
            if (cartDetails == null) return View("NotFound");

            await _service.RemoveFromCart(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromCart(long id)
        {
            var cartDetails = await _service.GetCartItemsByProductId(id);
            if (cartDetails == null) return View("NotFound");

            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(cartDetails);
        }

        [HttpPost, ActionName("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCartConfirmed(long id)
        {
            var cartDetails = await _service.GetCartItemsByProductId(id);
            if (cartDetails == null) return View("NotFound");

            await _service.RemoveAllFromCart(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
