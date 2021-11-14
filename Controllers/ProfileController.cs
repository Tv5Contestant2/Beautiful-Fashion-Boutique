using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ICartService _service;
        private readonly IProductCategoriesService _productCategoriesService;

        public ProfileController(ICartService service,
            IProductCategoriesService productCategoriesService)
        {
            _service = service;
            _productCategoriesService = productCategoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> Orders()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> Addresses()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }
    }
}
