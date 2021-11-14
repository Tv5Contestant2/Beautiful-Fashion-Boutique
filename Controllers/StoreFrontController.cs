using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class StoreFrontController : Controller
    {
        private readonly IProductsService _service;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly ICartService _cartService;

        public StoreFrontController(IProductsService service, 
            IProductCategoriesService productCategoriesService,
            ICartService cartService)
        {
            _service = service;
            _productCategoriesService = productCategoriesService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(result);
        }

        public async Task<IActionResult> AboutUs()
        {
            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> ContactUs()
        {
            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> Mens()
        {
            var data = await _service.GetAllProductsByGender((int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(data);
        }

        public async Task<IActionResult> Womens()
        {
            var data = await _service.GetAllProductsByGender((int)GenderEnum.Women);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(data);
        }

        public async Task<IActionResult> Trending()
        {
            var data = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;
            
            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(data);
        }

        public async Task<IActionResult> ShopAll()
        {
            var data = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(data);
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");

            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View(productDetails);
        }

    }
}
