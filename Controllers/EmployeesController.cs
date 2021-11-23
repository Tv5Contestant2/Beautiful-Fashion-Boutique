using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([Bind("EmployeeFirstName,EmployeeLastName,ImageFile")] Employees Employees)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid)
                return View(Employees);

            if (Employees.ImageFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    Employees.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Employees.Image = Convert.ToBase64String(fileBytes);
                }
            }

            Employees.DateCreated = DateTime.Now;

            _service.CreateEmployee(Employees);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var EmployeeDetails = await _service.GetEmployeeById(id);
            if (EmployeeDetails == null) return View("NotFound");

            await _service.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var EmployeeDetails = await _service.GetEmployeeById(id);
            if (EmployeeDetails == null) return View("NotFound");
            return View(EmployeeDetails);
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllEmployees();
            return View(data);
        }

        public async Task<IActionResult> UpdateEmployee(long id)
        {
            var EmployeeDetails = await _service.GetEmployeeById(id);
            if (EmployeeDetails == null) return View("NotFound");
            return View(EmployeeDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(long id, [Bind("Id,EmployeeFirstName,EmployeeLastName")] Employees Employees)
        {
            if (!ModelState.IsValid)
            {
                return View(Employees);
            }
            await _service.UpdateEmployee(id, Employees);
            return RedirectToAction(nameof(Index));
        }
    }
}