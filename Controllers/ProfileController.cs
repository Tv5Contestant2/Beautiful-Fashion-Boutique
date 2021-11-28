using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ProfileController : Controller
    {
        private readonly ICartService _service;
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public ProfileController(ICartService service,
            UserManager<User> userManager,
            IOrderService orderService)
        {
            _service = service;
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _service.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = await _orderService.GetAllOrdersByUser(userId);

            return View(result);
        }

        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _service.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = await _orderService.GetAllOrdersByUser(userId);

            return View(result);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _service.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = await _orderService.GetAllOrdersByUser(userId);

            return View(result);
        }

        public async Task<IActionResult> Addresses()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _service.GetCartItems(userId);
            ViewBag.Cart = cart;

            var result = await _orderService.GetAllOrdersByUser(userId);

            return View(result);
        }
    }
}
