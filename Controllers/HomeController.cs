using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartService _cartService;

        public HomeController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index","StoreFront");
        }

        public async Task<IActionResult> SignIn()
        {
            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        public async Task<IActionResult> UnderConstruction()
        {
            var cart = await _cartService.GetCacheCartItems(); //include cache id or user id
            ViewBag.Cart = cart;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("Username,EmployeePass")] Employees employees)
        {
            try
            {
                if (employees.Username.ToLower() == "administrator")
                {
                    return RedirectToAction("Index", "Administrator");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction();
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
