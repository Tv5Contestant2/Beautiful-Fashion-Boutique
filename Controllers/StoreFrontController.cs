﻿using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
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
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Products = products;

            return View(cart);
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
            var products = await _service.GetAllProductsByGender((int)GenderEnum.Men);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> Womens()
        {
            var products = await _service.GetAllProductsByGender((int)GenderEnum.Women);

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> Trending()
        {
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> ShopAll()
        {
            var products = await _service.GetAllProducts();

            var productCategories = await _productCategoriesService.GetAllProductCategories();
            ViewBag.ProductCategories = productCategories;

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Products = products;

            return View(cart);
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");

            var cart = new Cart();
            ViewBag.Cart = await _cartService.GetCacheCartItems(); //include cache id or user id;
            ViewBag.Product = productDetails;

            return View(cart);
        }

    }
}
