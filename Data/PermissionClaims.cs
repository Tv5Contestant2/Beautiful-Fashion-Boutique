using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce1.Data
{
    public static class PermissionClaims
    {
        public static async Task AddPermissionAndGenerateForModuleClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }

        public static async Task AddPermissionClaimView(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsViewForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }

        public static async Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Dashboard");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Customers");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Employees");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Roles");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Orders");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Returns");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Products");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "PromosAndDiscounts");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "AboutUs");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "ContactUs");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Reports");
        }

        public static async Task SeedClaimsForBasic(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Basic");
            await roleManager.AddPermissionClaimView(adminRole, "Dashboard");
            await roleManager.AddPermissionClaimView(adminRole, "Customers");
            await roleManager.AddPermissionClaimView(adminRole, "Employees");
            await roleManager.AddPermissionClaimView(adminRole, "Orders");
            await roleManager.AddPermissionClaimView(adminRole, "Returns");
            await roleManager.AddPermissionClaimView(adminRole, "Products");
            await roleManager.AddPermissionClaimView(adminRole, "PromosAndDiscounts");
            await roleManager.AddPermissionClaimView(adminRole, "AboutUs");
            await roleManager.AddPermissionClaimView(adminRole, "ContactUs");
            await roleManager.AddPermissionClaimView(adminRole, "Reports");
        }

        public static async Task SeedClaimsForEmployee(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Employee");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Dashboard");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Customers");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Employees");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Orders");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Returns");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Products");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "PromosAndDiscounts");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "AboutUs");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "ContactUs");
            await roleManager.AddPermissionAndGenerateForModuleClaim(adminRole, "Reports");
        }
    }
}