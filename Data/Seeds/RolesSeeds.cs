using ECommerce1.Data.Enums;
using ECommerce1.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ECommerce1.Data.Seeds
{
    public static class RolesSeeds
    {
        public static async Task SeedRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.Basic.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.Employee.ToString()));
        }
    }
}