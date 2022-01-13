using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Helper;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ActionResult> Index(string id)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), id);
            var role = await _roleManager.FindByIdAsync(id);
            model.RoleId = id;
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
        //public async Task<IActionResult> Update(PermissionViewModel model)
        //{
        //    var role = await _roleManager.FindByIdAsync(model.id);
        //    var claims = await _roleManager.GetClaimsAsync(role);
        //    foreach (var claim in claims)
        //    {
        //        await _roleManager.RemoveClaimAsync(role, claim);
        //    }
        //    var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
        //    foreach (var claim in selectedClaims)
        //    {
        //        await _roleManager.AddPermissionClaim(role, claim.Value);
        //    }
        //    return RedirectToAction("Index", new { id = model.id });
        //}
    }
}