using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _service;
        private readonly IProductCategoriesService _productCategoriesService;

        public CartController(ICartService service,
            IProductCategoriesService productCategoriesService)
        {
            _service = service;
            _productCategoriesService = productCategoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public IActionResult AddToCart()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(Cart cart)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid)
                return View(cart);
            
            _service.AddToCart(cart);

            return RedirectToAction(nameof(Index));
        }
    }
}
