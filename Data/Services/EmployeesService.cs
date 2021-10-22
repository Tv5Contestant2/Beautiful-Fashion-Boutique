using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public void CreateEmployee(Employees employees)
        {
            _context.Employees.Add(employees);
            _context.SaveChanges();
        }

        public Task<bool> DeleteEmployee(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employees>> GetAllEmployees()
        {
            var result = await _context.Employees.ToListAsync();

            return result;
        }

        public Employees GetEmployeeById(long Id)
        {
            throw new NotImplementedException();
        }

        public Employees UpdateEmployee(long Id, Employees employees)
        {
            throw new NotImplementedException();
        }
    }
}
