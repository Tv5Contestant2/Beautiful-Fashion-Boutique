using ECommerce1.Data;
using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEmailService _emailService;

        private readonly AppDBContext _context;

        private readonly UserManager<User> _userManager;

        public StoreFrontController( IAdministratorService administratorService
            , ICartService cartService
            , IProductsService service
            , IProductCategoriesService productCategoriesService
            , IUserService userService
            , UserManager<User> userManager
            , AppDBContext context
            , IEmailService emailService)
        {
            _service = service;
            _administratorService = administratorService;
            _productCategoriesService = productCategoriesService;
            _cartService = cartService;
            _userService = userService;
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index(int page = 1)
        {
            return View(new ProductViewModel());
        }

        public async Task<IActionResult> Home()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _service.GetFeaturedProductsOnSale();
            var productCategories = await _productCategoriesService.GetAllProductCategories();

            ViewBag.FacebookLink = _administratorService.GetFacebookLink();
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.ProductCategories = productCategories;
            ViewBag.Products = products;
            ViewBag.CustomersId = userId;

            await _userService.ArchiveUsers();
            await _userService.DeleteCustomers();

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

        public async Task<IActionResult> Mens(int categoryId = 0, int sizeId = 0, int colorId = 0, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var products = new List<Product>();

            if (categoryId == 0)
                products = await _service.GetAllProductsByGender(colorId, (int)GenderEnum.Men);
            else if (categoryId != 0 && sizeId != 0)
                products = await _service.GetProductsBySize(categoryId, sizeId, colorId, (int)GenderEnum.Men);
            else
                products = await _service.GetProductsByCategory(categoryId, colorId, (int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            var sizes = await _productCategoriesService.GetSizesPerCategory(categoryId);
            var colors = await _productCategoriesService.GetColors();
            var wishlists = await _cartService.GetWishlistItems(userId);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.Sizes = sizes;
            ViewBag.Colors = colors;

            var viewModel = new ProductViewModel
            {
                CartDetails = new CartDetails(),
                CurrentWishlists = wishlists,
                ItemPerPage = 12,
                Products = products.ToList(),
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Womens(int categoryId = 0, int sizeId = 0, int colorId = 0, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var products = new List<Product>();

            if (categoryId == 0)
                products = await _service.GetAllProductsByGender(colorId, (int)GenderEnum.Women);
            else if (categoryId != 0 && sizeId != 0)
                products = await _service.GetProductsBySize(categoryId, sizeId, colorId, (int)GenderEnum.Men);
            else
                products = await _service.GetProductsByCategory(categoryId, colorId, (int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            var sizes = await _productCategoriesService.GetSizesPerCategory(categoryId);
            var colors = await _productCategoriesService.GetColors();
            var wishlists = await _cartService.GetWishlistItems(userId);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.Sizes = sizes;
            ViewBag.Colors = colors;

            var viewModel = new ProductViewModel
            {
                CartDetails = new CartDetails(),
                CurrentWishlists = wishlists,
                ItemPerPage = 12,
                Products = products.ToList(),
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Trending(int categoryId = 0, int sizeId = 0, int colorId = 0, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var products = new List<Product>();

            if (categoryId == 0)
                products = await _service.GetLatestProducts(colorId);
            else if (categoryId != 0 && sizeId != 0)
                products = await _service.GetProductsBySize(categoryId, sizeId, colorId);
            else
                products = await _service.GetProductsByCategory(categoryId, colorId);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            var sizes = await _productCategoriesService.GetSizesPerCategory(categoryId);
            var colors = await _productCategoriesService.GetColors();
            var wishlists = await _cartService.GetWishlistItems(userId);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.Sizes = sizes;
            ViewBag.Colors = colors;

            var viewModel = new ProductViewModel
            {
                CartDetails = new CartDetails(),
                CurrentWishlists = wishlists,
                ItemPerPage = 12,
                Products = products.ToList(),
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ShopAll(int categoryId = 0, int sizeId = 0, int colorId = 0, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var products = new List<Product>();

            if (categoryId == 0)
                products = await _service.GetAllProducts(colorId);
            else if (categoryId != 0 && sizeId != 0)
                products = await _service.GetProductsBySize(categoryId, sizeId, colorId);
            else
                products = await _service.GetProductsByCategory(categoryId, colorId);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            var sizes = await _productCategoriesService.GetSizesPerCategory(categoryId);
            var colors = await _productCategoriesService.GetColors();
            var wishlists = await _cartService.GetWishlistItems(userId);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.CustomersId = userId;
            ViewBag.Products = products;
            ViewBag.ProductCategories = productCategories;
            ViewBag.Sizes = sizes;
            ViewBag.Colors = colors;

            var viewModel = new ProductViewModel
            {
                CartDetails = new CartDetails(),
                CurrentWishlists = wishlists,
                ItemPerPage = 12,
                Products = products.ToList(),
                CurrentPage = page
            };

            return View(viewModel);
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
            ViewBag.ContactUs = _administratorService.GetContactUs();

            return View(new MessageViewModel());
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
                Wishlists = new Wishlist(),
                Colors = productDetails.ProductVariants.Select(x => x.Color).Distinct(),
                Size = productDetails.ProductVariants.Select(x => x.Size).Distinct(),
        };


            var _result = viewModel.Size.FirstOrDefault();


            return View(viewModel);
        }

        public async Task<IActionResult> SendMessage(MessageViewModel viewModel)
        {
            await _emailService.SendMessage(viewModel);

            ViewBag.FacebookLink = _administratorService.GetFacebookLink();
            return View("MessageSuccess");
        }

    }
}