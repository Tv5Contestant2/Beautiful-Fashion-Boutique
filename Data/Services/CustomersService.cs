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

        public Customers InitializeCustomer()
        {
            var _result = new Customers()
            {
                Genders = _context.Genders.ToList()
            };

            return _result;

        }

        public void CreateCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public async Task<Customers> UpdateCustomer(long id, Customers customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task DeleteCustomer(long id)
        {
            var result = _context.Customers.FirstOrDefault(customer => customer.CustomerID == id);
            _context.Customers.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customers>> GetAllCustomers()
        {
            var result = await _context.Customers.Where(x => !x.IsBlock).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Customers>> GetAllBlockCustomers()
        {
            var result = await _context.Customers.Where(x => x.IsBlock).ToListAsync();
            return result;
        }

        public async Task<Customers> GetCustomerById(long id)
        {
            var result = await _context.Customers.ToListAsync();
            return result.Find(x => x.CustomerID == id);
        }

        public async Task<IEnumerable<Customers>> GetAllCustomersByGender(int genderId)
        {
            var result = await _context.Customers.ToListAsync();
            return result.Where(x => x.GenderId == genderId).ToList();
        }
    }
}
