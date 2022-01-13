using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class RolesController : Controller
    {
        private readonly ICommonServices _commonService;
        private readonly IRolesService _rolesService;
        private readonly UserManager<User> _userManager;

        public RolesController(IRolesService rolesService, ICommonServices commonService, UserManager<User> userManager)
        {
            _rolesService = rolesService;
            _commonService = commonService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var data = await _rolesService.GetAllRoles();

            var viewModel = new RoleViewModel
            {
                ItemPerPage = 10,
                RoleList = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Bind] RoleViewModel model)
        {
            var _user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(_user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(_user).Result.FirstOrDefault();
            ViewBag.Role = role;

            if (!ModelState.IsValid) return View(model);

            var _result = await _rolesService.AddRole(model.Role);
            if (!_result.Item1) ModelState.AddModelError(string.Empty, _result.Item2);

            if (!ModelState.IsValid) return View("CreateRole", model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewRole(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var result = await _rolesService.FindByIdAsync(id);

            var model = new RoleViewModel
            {
                Id = result.Id,
                Role = result.Name
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var result = await _rolesService.FindByIdAsync(id);

            var model = new RoleViewModel
            {
                Id = result.Id,
                Role = result.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole([Bind] RoleViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            if (!ModelState.IsValid) return View(model);

            var _result = await _rolesService.UpdateRole(model.Id, model.Role);
            if (!_result.Item1) ModelState.AddModelError(string.Empty, _result.Item2);

            if (!ModelState.IsValid) return View("CreateRole", model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _model = await _rolesService.FindByIdAsync(id);
            if (_model == null) return RedirectToAction("Error", "Home");

            return View(_model);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _model = await _rolesService.FindByIdAsync(id);
            if (_model == null) return RedirectToAction("Error", "Home");

            var result = await _rolesService.DeleteRole(id);
            if (!result.Item1) return RedirectToAction("Error", "Home");

            return RedirectToAction(nameof(Index));
        }
    }
}