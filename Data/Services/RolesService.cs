using ECommerce1.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private bool _IsResult = true;
        private string _errMessage;

        public RolesService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoles() => await _roleManager.Roles.ToListAsync();

        public async Task<(bool, string)> AddRole(string roleName)
        {
            try
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            catch (Exception ex)
            {
                _IsResult = false;
                _errMessage = ex.ToString();
            }

            return (_IsResult, _errMessage);
        }

        public async Task<(bool, string)> UpdateRole(string id, string name)
        {
            try
            {
                var model = await FindByIdAsync(id);
                model.Name = name;

                await _roleManager.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                _IsResult = false;
                _errMessage = ex.ToString();
            }

            return (_IsResult, _errMessage);
        }

        public async Task<(bool, string)> DeleteRole(string id)
        {
            try
            {
                var model = await FindByIdAsync(id);
                await _roleManager.DeleteAsync(model);
            }
            catch (Exception ex)
            {
                _IsResult = false;
                _errMessage = ex.ToString();
            }

            return (_IsResult, _errMessage);
        }

        public async Task<IdentityRole> FindByIdAsync(string id) => await _roleManager.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}