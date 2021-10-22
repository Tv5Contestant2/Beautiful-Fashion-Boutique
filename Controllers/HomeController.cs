using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index","StoreFront");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult UnderConstruction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,EmployeePass")] Employees employees)
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
        public IActionResult Registration()
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
