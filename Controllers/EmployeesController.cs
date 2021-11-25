using ECommerce1.Data.Services.Interfaces;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _service;
        private readonly ICommonServices _commonServices;

        public EmployeesController(IEmployeesService service, ICommonServices commonServices)
        {
            _service = service;
            _commonServices = commonServices;
        }

        public IActionResult CreateEmployee()
        {
            var _initModel = new EmployeeViewModel()
            {
                Image = _commonServices.NoImage,
                Birthday = DateTime.Now
            };

            return View(_initModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([Bind] EmployeeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);

            if (!ModelState.IsValid) return View(model);

            model.DateCreated = DateTime.Now;

            var _result = await _service.CreateEmployee(model);
            if (!_result.Item1)
            {
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in _result.Item2)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                if (!ModelState.IsValid)
                {
                    return View("CreateEmployee", model);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllEmployees();
            return View(data);
        }

        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var _employee = await _service.GetEmployeeById(id);
            if (_employee == null) return View("NotFound");

            return View(_employee);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
        {
            var _employee = await _service.GetEmployeeById(id);
            if (_employee == null) return View("NotFound");

            await _service.DeleteEmployee(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var _employee = await _service.GetEmployeeById(id);
            var _employeeViewModel = new EmployeeViewModel
            {
                AddressBaranggay = _employee.AddressBaranggay,
                AddressBlock = _employee.AddressBlock,
                AddressCity = _employee.AddressCity,
                AddressLot = _employee.AddressLot,
                Birthday = (DateTime)_employee.Birthday,
                ContactNumber = _employee.ContactNumber,
                DateCreated = (DateTime)_employee.DateCreated,
                Email = _employee.Email,
                FirstName = _employee.FirstName,
                Id = _employee.Id,
                Image = _employee.Image,
                LastName = _employee.LastName,
            };

            return View(_employeeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee([Bind] EmployeeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);
            if (string.IsNullOrEmpty(model.Password)) //Do not update password if empty
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.UpdateEmployee(model);
            return RedirectToAction(nameof(Index));
        }
    }
}