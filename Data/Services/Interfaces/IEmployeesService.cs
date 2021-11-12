using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IEmployeesService
    {
        public void CreateEmployee(Employees employee);

        public Task<Employees> UpdateEmployee(long id, Employees employee);

        public Task DeleteEmployee(long id);

        public Task<IEnumerable<Employees>> GetAllEmployees();

        public Task<Employees> GetEmployeeById(long id);
    }
}
