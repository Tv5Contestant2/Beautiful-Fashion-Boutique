using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class StoreFrontController : Controller
    {
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

        public IActionResult Mens()
        {
            return View();
        }

        public IActionResult Womens()
        {
            return View();
        }

        public IActionResult Trending()
        {
            return View();
        }

        public IActionResult ShopAll()
        {
            return View();
        }
    }
}
