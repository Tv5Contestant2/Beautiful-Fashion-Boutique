using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<(bool, string)> AddRole(string roleName);

        Task<IdentityRole> FindByIdAsync(string id);

        Task<(bool, string)> UpdateRole(string id, string name);

        Task<(bool, string)> DeleteRole(string id);
    }
}