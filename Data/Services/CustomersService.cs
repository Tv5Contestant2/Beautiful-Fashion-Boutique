using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly AppDBContext _context;
        public CustomersService(AppDBContext context)
        {
            _context = context;
        }

        public void CreateCustomer(Customers customers)
        {
            _context.Customers.Add(customers);
            _context.SaveChanges();
        }

        public Task<bool> DeleteCustomer(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customers>> GetAllCustomers()
        {
            var result = await _context.Customers.ToListAsync();

            return result;
        }

        public Customers GetCustomerById(long Id)
        {
            throw new NotImplementedException();
        }

        public Customers UpdateCustomer(long Id, Customers customers)
        {
            throw new NotImplementedException();
        }

        public Customers InitializeCustomer()
        {
            var _result = new Customers()
            {
                Genders = _context.Genders.ToList()
            };

            return _result;

        }
    }
}
