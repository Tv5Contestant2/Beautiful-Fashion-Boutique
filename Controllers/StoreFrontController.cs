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
        private readonly IEmailService _emailService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IProductsService _productService;
        private readonly IStoreFrontService _service;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public StoreFrontController(IAdministratorService administratorService
            , ICartService cartService
            , IEmailService emailService
            , IProductsService productService
            , IProductCategoriesService productCategoriesService
            , IStoreFrontService service
            , IUserService userService
            , UserManager<User> userManager)
        {
            _administratorService = administratorService;
            _cartService = cartService;
            _emailService = emailService;
            _productCategoriesService = productCategoriesService;
            _productService = productService;
            _service = service;
            _userService = userService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(new ProductViewModel());
        }

        public async Task<IActionResult> Home()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var products = await _productService.GetFeaturedProductsOnSale();

            ViewBag.Products = products;
            ViewBag.CartCount = await _cartService.GetCartCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            try
            {
                await _userService.ArchiveUsers();

                if (!string.IsNullOrEmpty(userId))
                    await _userService.UpdateLastLoggedIn(userId);
            }
            catch
            {
                return View();
            }

            return View();
        }

        public async Task<IActionResult> Mens(FilterViewModel filterViewModel, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            filterViewModel.GenderId = (int)GenderEnum.Men;

            var viewModel = await _service.InitializeStoreFront(userId, page, filterViewModel);
            viewModel.PageTitle = "Men's Collection";
            viewModel.PageDescription = "Prepare to mesmerize. Check this collection meant just for you.";

            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View("Index", viewModel);
        }

        public async Task<IActionResult> Womens(FilterViewModel filterViewModel, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            filterViewModel.GenderId = (int)GenderEnum.Women;

            var viewModel = await _service.InitializeStoreFront(userId, page, filterViewModel);
            viewModel.PageTitle = "Women's Collection";
            viewModel.PageDescription = "Ladies, be a trendsetter with these items.";

            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View("Index", viewModel);
        }

        public async Task<IActionResult> Trending(FilterViewModel filterViewModel, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var viewModel = await _service.InitializeStoreFront(userId, page, filterViewModel);
            viewModel.PageTitle = "Trending / New Arrivals";
            viewModel.PageDescription = "Hottest and latest items on our stash.";

            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View("Index", viewModel);
        }

        public async Task<IActionResult> ShopAll(FilterViewModel filterViewModel, int page = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var viewModel = await _service.InitializeStoreFront(userId, page, filterViewModel);
            viewModel.PageTitle = "Shop All";
            viewModel.PageDescription = "Everything and anything under the sun.";

            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View("Index", viewModel);
        }

        public IActionResult AboutUs()
        {
            ViewBag.CartCount = 0; //await _cartService.GetCartTotalQty(userId);

            var result = _administratorService.GetAboutUs();
            return View(result);
        }

        public IActionResult ContactUs()
        {
            ViewBag.CartCount = 0; //await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = 0; //await _cartService.GetWishlistCount(userId);
            ViewBag.ContactUs = _administratorService.GetContactUs();

            return View(new MessageViewModel());
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var viewModel = await _service.InitializeViewProduct(id, userId);
            ViewBag.CartCount = viewModel.CartCount;

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