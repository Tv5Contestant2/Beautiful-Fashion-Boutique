using Microsoft.AspNetCore.Mvc;

namespace ECommerce1.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart()
        {
            return View();
        }
    }
}
