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

        public async Task<IActionResult> BlockCustomers()
        {
            var data = await _service.GetAllBlockCustomers();
            return View(data);
        }

        public async Task<IActionResult> ShowCustomers()
        {
            await Task.Delay(0);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult CreateCustomer()
        {
            var viewModel = _service.InitializeCustomer();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerFirstName,CustomerLastName,ImageFile")] Customers customers)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid) {
                var _result = _service.InitializeCustomer();
                if (customers.Genders == null) customers.Genders = _result.Genders;
                return View(customers);
            }
                

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

        public async Task<IActionResult> UpdateCustomer(long id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return View("NotFound");

            ViewBag.CustomerRepo = _service.InitializeCustomer();
            
            return View(customerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(long id, Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return View(customers);
            }
            await _service.UpdateCustomer(id, customers);

            return RedirectToAction(nameof(Index));
        }

       


        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return View("NotFound");
            return View(customerDetails);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return View("NotFound");

            await _service.DeleteCustomer(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BlockCustomer(long id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return View("NotFound");

            return View(customerDetails);
        }

        [HttpPost, ActionName("BlockCustomer")]
        public async Task<IActionResult> BlockConfirmed(long id)
        {
            var customer = await _service.GetCustomerById(id);
            if (customer == null) return View("NotFound");

            customer.IsBlock = true;

            await _service.UpdateCustomer(id, customer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnBlockCustomer(long id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return View("NotFound");

            return View(customerDetails);
        }

        [HttpPost, ActionName("UnBlockCustomer")]
        public async Task<IActionResult> UnBlockConfirmed(long id)
        {
            var customer = await _service.GetCustomerById(id);
            if (customer == null) return View("NotFound");

            customer.IsBlock = false;

            await _service.UpdateCustomer(id, customer);

            return RedirectToAction(nameof(Index));
        }


    }
}
