using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly AppDBContext _context;
        public EmployeesService(AppDBContext context)
        {
            _context = context;
        }

        public void CreateEmployee(Employees employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public async Task<Employees> UpdateEmployee(long id, Employees employee)
        {
            _context.Update(id);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteEmployee(long id)
        {
            var result = _context.Employees.FirstOrDefault(employee => employee.EmployeeID == id);
            _context.Employees.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employees>> GetAllEmployees()
        {
            var result = await _context.Employees.ToListAsync();
            return result;
        }

        public async Task<Employees> GetEmployeeById(long id)
        {
            var result = await _context.Employees.ToListAsync();
            return result.Find(x => x.EmployeeID == id);
        }
    }
}
