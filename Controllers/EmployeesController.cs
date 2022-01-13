using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _service;
        private readonly ICommonServices _commonServices;
        public readonly IRandomPasswordService _randomPasswordService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public EmployeesController(IEmployeesService service
            , ICommonServices commonServices
            , IRandomPasswordService randomPasswordService
            , UserManager<User> userManager
            , RoleManager<IdentityRole> roleManager
            , IEmailService emailService
            )
        {
            _service = service;
            _commonServices = commonServices;
            _randomPasswordService = randomPasswordService;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> CreateEmployee()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonServices.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _roleList = _roleManager.Roles.Where(x => x.Name.ToLower() != RolesEnum.Admin.ToString().ToLower());
            var _selectedDefaultRoleId = _roleManager.Roles.Where(x => x.Name.ToLower() == RolesEnum.Employee.ToString().ToLower()).FirstOrDefault().Id;
            var _initModel = new EmployeeViewModel()
            {
                Image = _commonServices.NoImage,
                Birthday = DateTime.Now,
                RoleId = _selectedDefaultRoleId,
                RoleList = _roleList
            };

            return View(_initModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([Bind] EmployeeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);

            if (model.IsAutoGeneratePassword)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                model.Password = _randomPasswordService.GenerateRandomPassword();
            }

            if (!ModelState.IsValid) return View(model);

            model.DateCreated = DateTime.Now;

            // Copy data from RegisterViewModel to IdentityUser
            var user = new User
            {
                AddressBaranggay = model.AddressBaranggay,
                AddressBlock = model.AddressBlock,
                AddressCity = model.AddressCity,
                AddressLot = model.AddressLot,
                Birthday = model.Birthday,
                ContactNumber = model.ContactNumber,
                DateCreated = model.DateCreated,
                Email = model.Email,
                FirstName = model.FirstName,
                Image = model.Image,
                IsEmployee = true,
                IsFirstTimeLogin = false,
                LastName = model.LastName,
                UserName = model.Email,
                LastLoggedIn = DateTime.Now
            };

            // Store user data in AspNetUsers database table
            var result = await _userManager.CreateAsync(user, model.Password);

            // If user is successfully created, sign-in the user using
            // SignInManager and redirect to index action of HomeController
            if (result.Succeeded)
            {
                var _selectedRole = await _roleManager.FindByIdAsync(model.RoleId);
                await _userManager.AddToRoleAsync(user, _selectedRole.Name);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, token = token }, Request.Scheme);

                // send email confirmation
                await _emailService.SendConfirmationEmail(model.Email, confirmationLink, model.Password);
            }
            else
            {
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                if (!ModelState.IsValid) return View("CreateEmployee", model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonServices.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var data = await _service.GetAllEmployees();

            var viewModel = new HomeUserViewModel
            {
                ItemPerPage = 10,
                Users = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonServices.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _employee = await _service.GetEmployeeById(id);
            if (_employee == null) return RedirectToAction("Error", "Home");

            return View(_employee);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
        {
            var _employee = await _service.GetEmployeeById(id);
            if (_employee == null) return RedirectToAction("Error", "Home");

            await _service.DeleteEmployee(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonServices.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _employee = await _service.GetEmployeeById(id);
            var _employeeRoleName = (await _userManager.GetRolesAsync(_employee)).FirstOrDefault();
            var _employeeRole = _roleManager.Roles.Where(x => x.Name == _employeeRoleName).FirstOrDefault();

            var _roleList = _roleManager.Roles.Where(x => x.Name.ToLower() != RolesEnum.Admin.ToString().ToLower());
            var _employeeViewModel = new EmployeeViewModel
            {
                AddressBaranggay = string.IsNullOrEmpty(_employee.AddressBaranggay) ? string.Empty : _employee.AddressBaranggay,
                AddressBlock = string.IsNullOrEmpty(_employee.AddressBlock) ? string.Empty : _employee.AddressBlock,
                AddressCity = string.IsNullOrEmpty(_employee.AddressCity) ? string.Empty : _employee.AddressCity,
                AddressLot = string.IsNullOrEmpty(_employee.AddressLot) ? string.Empty : _employee.AddressLot,
                Birthday = _employee.Birthday,
                ContactNumber = _employee.ContactNumber,
                DateCreated = _employee.DateCreated,
                Email = _employee.Email,
                FirstName = string.IsNullOrEmpty(_employee.FirstName) ? string.Empty : _employee.FirstName,
                Id = _employee.Id,
                Image = _employee.Image,
                LastName = string.IsNullOrEmpty(_employee.LastName) ? string.Empty : _employee.LastName,
                RoleList = _roleList,
                RoleId = _employeeRole.Id
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