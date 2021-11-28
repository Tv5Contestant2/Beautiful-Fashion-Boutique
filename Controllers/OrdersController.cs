using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        private readonly IOrderService _service;
        private readonly UserManager<User> _userManager;

        public OrdersController(IOrderService service,
            UserManager<User> userManager)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllOrders();
            return View(result);
        }
    }
}
