using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce1.Data
{
    public static class PermissionClaims
    {
        public static async Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            await roleManager.AddPermissionClaim(adminRole, "Dashboard");
            await roleManager.AddPermissionClaim(adminRole, "Customers");
            await roleManager.AddPermissionClaim(adminRole, "Employees");
            await roleManager.AddPermissionClaim(adminRole, "Roles");
            await roleManager.AddPermissionClaim(adminRole, "Orders");
            await roleManager.AddPermissionClaim(adminRole, "Returns");
            await roleManager.AddPermissionClaim(adminRole, "Products");
            await roleManager.AddPermissionClaim(adminRole, "Promos");
            await roleManager.AddPermissionClaim(adminRole, "AboutUs");
            await roleManager.AddPermissionClaim(adminRole, "ContactUs");
            await roleManager.AddPermissionClaim(adminRole, "Reports");
        }

        public static async Task SeedClaimsForEmployee(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Employee");
            await roleManager.AddPermissionClaim(adminRole, "Dashboard");
            await roleManager.AddPermissionClaim(adminRole, "Customers");
            await roleManager.AddPermissionClaim(adminRole, "Employees");
            await roleManager.AddPermissionClaim(adminRole, "Orders");
            await roleManager.AddPermissionClaim(adminRole, "Returns");
            await roleManager.AddPermissionClaim(adminRole, "Products");
            await roleManager.AddPermissionClaim(adminRole, "Promos");
            await roleManager.AddPermissionClaim(adminRole, "AboutUs");
            await roleManager.AddPermissionClaim(adminRole, "ContactUs");
            await roleManager.AddPermissionClaim(adminRole, "Reports");
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
            await roleManager.AddPermissionClaimView(adminRole, "Promos");
            await roleManager.AddPermissionClaimView(adminRole, "AboutUs");
            await roleManager.AddPermissionClaimView(adminRole, "ContactUs");
            await roleManager.AddPermissionClaimView(adminRole, "Reports");
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
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
    }
}