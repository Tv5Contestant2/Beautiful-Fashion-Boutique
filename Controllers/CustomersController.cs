using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomersService _service;
        public CustomersController(ICustomersService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllCustomers();
            return View(data);
        }

        public async Task<IActionResult> CreateCustomer()
        {
            var viewModel = _service.InitializeCustomer();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerFirstName,CustomerLastName,ImageFile")] Customers customers)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid)
                return View(customers);

            if (customers.ImageFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    customers.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    customers.Image = Convert.ToBase64String(fileBytes);
                }
            }

            customers.DateCreated = DateTime.Now;

            _service.CreateCustomer(customers);
            return RedirectToAction(nameof(Index));
        }
    }
}
