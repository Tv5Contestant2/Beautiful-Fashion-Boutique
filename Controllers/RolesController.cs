using ECommerce1.Data.Services.Interfaces;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
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
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Bind] RoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var _result = await _rolesService.AddRole(model.Role);
            if (!_result.Item1) ModelState.AddModelError(string.Empty, _result.Item2);

            if (!ModelState.IsValid) return View("CreateRole", model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewRole(string id)
        {
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
            if (!ModelState.IsValid) return View(model);

            var _result = await _rolesService.UpdateRole(model.Id, model.Role);
            if (!_result.Item1) ModelState.AddModelError(string.Empty, _result.Item2);

            if (!ModelState.IsValid) return View("CreateRole", model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var _model = await _rolesService.FindByIdAsync(id);
            if (_model == null) return RedirectToAction("Error", "Home");

            return View(_model);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var _model = await _rolesService.FindByIdAsync(id);
            if (_model == null) return RedirectToAction("Error", "Home");

            var result = await _rolesService.DeleteRole(id);
            if (!result.Item1) return RedirectToAction("Error", "Home");

            return RedirectToAction(nameof(Index));
        }
    }
}