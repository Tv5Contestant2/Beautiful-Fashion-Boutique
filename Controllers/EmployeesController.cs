using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _service;
        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllEmployees();
            return View(data);
        }

        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([Bind("EmployeeFirstName,EmployeeLastName,ImageFile")] Employees employees)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid)
                return View(employees);

            if (employees.ImageFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    employees.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    employees.Image = Convert.ToBase64String(fileBytes);
                }
            }

            employees.DateCreated = DateTime.Now;

            _service.CreateEmployee(employees);
            return RedirectToAction(nameof(Index));
        }
    }
}
