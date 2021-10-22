using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IEmployeesService
    {
        public Task<IEnumerable<Employees>> GetAllEmployees();
        public Employees GetEmployeeById(long Id);
        public void CreateEmployee(Employees employees);
        public Employees UpdateEmployee(long Id, Employees employees);
        public Task<bool> DeleteEmployee(long Id);
    }
}
