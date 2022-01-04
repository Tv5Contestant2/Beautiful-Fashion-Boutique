using Adyen.Model.Checkout;
using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class WishlistController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public WishlistController(
            UserManager<User> userManager
            , ICartService cartService)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var viewModel = await _cartService.InitializeWishlist(userId);
            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View(viewModel);
        }


        //public async Task<IActionResult> AddToWishlist(ProductViewModel viewModel)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    if (userId == null) return RedirectToAction("SignIn", "Home");

        //    var wishlistItems = await _service.GetWishlistItemsByProductId(viewModel.Wishlists.ProductId, userId);

        //    if (wishlistItems == null)
        //        _service.AddToWishlist(viewModel.Wishlists, viewModel.CartDetails);
        //    else
        //    {
        //        await _service.RemoveFromWishlist(viewModel.Wishlists.ProductId, userId);
        //    }

        //    return Redirect(Request.Headers["Referer"].ToString());
        //}

        //public async Task<IActionResult> AddToCartFromWishlist(long id)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    if (userId == null) return RedirectToAction("SignIn", "Home");

        //    var _wishlistItems = await _service.GetWishlistItemsByProductId(id, userId);
        //    if (_wishlistItems == null) return RedirectToAction("Error", "Home");

        //    var cartDetails = new CartDetails();
        //    cartDetails.CustomersId = _wishlistItems.CustomersId;
        //    cartDetails.ProductId = _wishlistItems.ProductId;
        //    cartDetails.Quantity = 1;
        //    cartDetails.ColorId = _wishlistItems.ColorId;
        //    cartDetails.SizeId = _wishlistItems.SizeId;


        //    _service.AddToCartItems(cartDetails);

        //    // Cart
        //    var cart = await _service.GetCart(userId);
        //    if (cart == null)
        //    {
        //        var _cart = new Cart();
        //        _cart.CustomersId = userId;

        //        await _service.CreateCart(_cart);
        //    }

        //    await _service.RemoveFromWishlist(id, userId);

        //    return Redirect(Request.Headers["Referer"].ToString());
        //}        
    }
}