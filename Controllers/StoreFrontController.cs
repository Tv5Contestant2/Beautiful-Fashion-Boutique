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
    public class StoreFrontController : Controller
    {
        private readonly IAdministratorService _administratorService;
        private readonly ICartService _cartService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IProductsService _service;
        private readonly IUserService _userService;

        private readonly UserManager<User> _userManager;

        public StoreFrontController(IAdministratorService administratorService
            , ICartService cartService
            , IProductsService service
            , IProductCategoriesService productCategoriesService
            , IUserService userService
            , UserManager<User> userManager)
        {
            _service = service;
            _administratorService = administratorService;
            _productCategoriesService = productCategoriesService;
            _cartService = cartService;
            _userService = userService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(new CartDetails());
        }

        public async Task<IActionResult> Home()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetFeaturedProductsOnSale();
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.ProductCategories = productCategories;
            ViewBag.Products = products;
            ViewBag.CustomersId = userId;

            await _userService.ArchiveUsers();

            try
            {

                if (!string.IsNullOrEmpty(userId))
                    await _userService.UpdateLastLoggedIn(userId);
            }
            catch
            {
                return View();
            }

            return View();
        }

        public async Task<IActionResult> Mens()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetAllProductsByGender((int)GenderEnum.Men);
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.PageTitle = "Men's Collection";
            ViewBag.PageDescription = "Prepare to mesmerize. Check this collection meant just for you.";

            return View("Index");
        }

        public async Task<IActionResult> Womens()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetAllProductsByGender((int)GenderEnum.Women);
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.PageTitle = "Women's Collection";
            ViewBag.PageDescription = "Ladies, be a trendsetter with these items.";

            return View("Index");
        }

        public async Task<IActionResult> Trending()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetAllProducts();
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.PageTitle = "Trending / New Arrivals";
            ViewBag.PageDescription = "Hottest and latest items on our stash.";

            return View("Index");
        }

        public async Task<IActionResult> ShopAll()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetAllProducts();
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.PageTitle = "Shop All";
            ViewBag.PageDescription = "Everything and anything under the sun.";

            return View("Index");
        }

        public async Task<IActionResult> AboutUs()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);

            var result = _administratorService.GetAboutUs();
            return View(result);
        }

        public async Task<IActionResult> ContactUs()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = _administratorService.GetContactUs();
            return View(result);
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return RedirectToAction("Error", "Home");

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.IsProductExist = _cartService.CheckIfExistInWishlist(id, userId);

            ViewBag.CustomersId = userId;
            ViewBag.Product = productDetails;

            var productReviews = await _service.GetProductReviews(id);

            var viewModel = new ProductViewModel()
            {
                CartDetails = new CartDetails(),
                ProductReviews = productReviews, 
                Wishlists = new Wishlist()
            };

            return View(viewModel);
        }

    }
}