using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Helper;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        private readonly ICommonServices _commonService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionController(ICommonServices commonService
            , UserManager<User> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            _commonService = commonService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var _role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = _role;

            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();

            //allPermissions.GetPermissions(typeof(Permissions.Dashboard), id);
            allPermissions.GetPermissions(typeof(Permissions.Customers), id);
            allPermissions.GetPermissions(typeof(Permissions.Employees), id);
            allPermissions.GetPermissions(typeof(Permissions.Roles), id);
            allPermissions.GetPermissions(typeof(Permissions.Orders), id);
            allPermissions.GetPermissions(typeof(Permissions.Returns), id);
            allPermissions.GetPermissions(typeof(Permissions.Products), id);
            allPermissions.GetPermissions(typeof(Permissions.PromosAndDiscounts), id);
            allPermissions.GetPermissions(typeof(Permissions.Reports), id);
            allPermissions.GetPermissions(typeof(Permissions.AboutUs), id);
            allPermissions.GetPermissions(typeof(Permissions.ContactUs), id);

            var role = await _roleManager.FindByIdAsync(id);
            model.RoleId = id;
            model.RoleName = role.Name;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePermission([Bind] PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return RedirectToAction("Index", "Roles");
        }
    }
}