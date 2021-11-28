using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
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
        private readonly UserManager<User> _userManager;

        public StoreFrontController(IProductsService service,
            IProductCategoriesService productCategoriesService,
            IAdministratorService administratorService,
            UserManager<User> userManager,
            ICartService cartService)
        {
            _service = service;
            _administratorService = administratorService;
            _productCategoriesService = productCategoriesService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> AboutUs()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = _administratorService.GetAboutUs();
            return View(result);
        }

        public async Task<IActionResult> ContactUs()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = _administratorService.GetContactUs();
            return View(result);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> Mens()
        {
            var products = await _service.GetAllProductsByGender((int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> ShopAll()
        {
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> Trending()
        {
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Product = productDetails;

            return View(cart);
        }

        public async Task<IActionResult> Womens()
        {
            var products = await _service.GetAllProductsByGender((int)GenderEnum.Women);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.Cart = await _cartService.GetCartItems(userId);
            ViewBag.Products = products;

            return View(cart);
        }
    }
}