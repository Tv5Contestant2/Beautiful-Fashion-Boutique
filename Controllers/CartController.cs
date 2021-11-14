﻿using ECommerce1.Data.Services;
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
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> DeliveryMethod()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> PaymentMethod()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> OrderReview()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> OrderConfirmed()
        {
            var cart = await _service.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }
        public IActionResult AddToCart(long productId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(long productId, int quantity)
        {
            await Task.Delay(0);

            if (productId == 0)
                return View();

            var _cart = new Cart()
            {
                ProductId = 1,
                Quantity = 2
            };
            
            _service.AddToCart(_cart);

            return RedirectToAction(nameof(Index));
        }
    }
}
