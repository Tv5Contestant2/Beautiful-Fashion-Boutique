using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _service;
        public readonly ICommonServices _commonServices;
        private readonly IUserService _userService;

        public CustomersController(ICustomersService service, ICommonServices commonServices, IUserService userService)
        {
            _service = service;
            _commonServices = commonServices;
            _userService = userService;
        }

        [HttpPost, ActionName("BlockCustomer")]
        public async Task<IActionResult> BlockConfirmed(string id)
        {
            var customer = await _service.GetCustomerById(id);
            if (customer == null) return RedirectToAction("Error", "Home");

            customer.IsBlock = true;

            await _service.UpdateCustomer(customer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BlockCustomer(string id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return RedirectToAction("Error", "Home");

            return View(customerDetails);
        }

        public async Task<IActionResult> ArchivedCustomers(int page = 1)
        {
            var data = await _service.GetAllArchivedCustomers();

            var viewModel = new HomeUserViewModel
            {
                ItemPerPage = 10,
                Users = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> BlockCustomers(int page = 1)
        {
            var data = await _service.GetAllBlockCustomers();

            var viewModel = new HomeUserViewModel
            {
                ItemPerPage = 10,
                Users = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public IActionResult CreateCustomer()
        {
            var viewModel = _service.InitializeCustomer();
            if (string.IsNullOrEmpty(viewModel.Image)) viewModel.Image = _commonServices.NoImage;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([Bind] CustomerViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);

            if (!ModelState.IsValid)
            {
                model.Genders = await _service.GetGenders();
                return View(model);
            }

            model.DateCreated = DateTime.Now;

            var _result = await _service.CreateCustomer(model);
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
                    return View("CreateCustomer", model);
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

        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var _customer = await _service.GetCustomerById(id);
            if (_customer == null) return RedirectToAction("Error", "Home");

            return View(_customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerConfirmed(string id)
        {
            var _employee = await _service.GetCustomerById(id);
            if (_employee == null) return RedirectToAction("Error", "Home");

            await _service.DeleteCustomer(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            await _userService.ArchiveUsers();

            var data = await _service.GetAllCustomers();

            var viewModel = new HomeUserViewModel
            {
                ItemPerPage = 10,
                Users = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ShowCustomers()
        {
            await Task.Delay(0);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("UnBlockCustomer")]
        public async Task<IActionResult> UnBlockConfirmed(string id)
        {
            var customer = await _service.GetCustomerById(id);
            if (customer == null) return RedirectToAction("Error", "Home");

            customer.IsBlock = false;

            await _service.UpdateCustomer(customer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnBlockCustomer(string id)
        {
            var customerDetails = await _service.GetCustomerById(id);
            if (customerDetails == null) return RedirectToAction("Error", "Home");

            return View(customerDetails);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(string id)
        {
            var _customer = await _service.GetCustomerById(id);
            var _customerViewModel = new CustomerViewModel
            {
                AddressBaranggay = _customer.AddressBaranggay,
                AddressBlock = _customer.AddressBlock,
                AddressCity = _customer.AddressCity,
                AddressLot = _customer.AddressLot,
                Birthday = (DateTime)_customer.Birthday,
                ContactNumber = _customer.ContactNumber,
                //DateCreated = (DateTime)_customer.DateCreated,
                Email = _customer.Email,
                FirstName = _customer.FirstName,
                GenderId = _customer.GenderId,
                Genders = await _service.GetGenders(),
                Id = _customer.Id,
                Image = _customer.Image,
                LastName = _customer.LastName,
            };

            return View(_customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer([Bind] CustomerViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);
            if (string.IsNullOrEmpty(model.Password)) //Do not update password if empty
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                _service.InitializeCustomer(model);
                return RedirectToAction("Index", "Profile", model);
                //return View(model);
            }

            await _service.UpdateCustomer(model);
            return RedirectToAction(nameof(Index));
        }
    }
}