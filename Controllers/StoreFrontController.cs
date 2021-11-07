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

        public StoreFrontController(IProductsService service, 
            IProductCategoriesService productCategoriesService)
        {
            _service = service;
            _productCategoriesService = productCategoriesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public async Task<IActionResult> Mens()
        {
            var data = await _service.GetAllProductsByGender((int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            return View(data);
        }

        public async Task<IActionResult> Womens()
        {
            var data = await _service.GetAllProductsByGender((int)GenderEnum.Women);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            return View(data);
        }

        public async Task<IActionResult> Trending()
        {
            var data = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            return View(data);
        }

        public async Task<IActionResult> ShopAll()
        {
            var data = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            return View(data);
        }
    }
}
