using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IEmployeesService
    {
        public Task<(bool, IEnumerable<IdentityError>)> CreateEmployee(EmployeeViewModel model);

        public Task UpdateEmployee(EmployeeViewModel model);

        public Task DeleteEmployee(string id);

        public Task<IEnumerable<User>> GetAllEmployees();

        public Task<User> GetEmployeeById(string id);
    }
}