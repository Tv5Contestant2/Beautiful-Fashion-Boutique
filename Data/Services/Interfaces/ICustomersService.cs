using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICustomersService
    {
        Customers InitializeCustomer();

        public void CreateCustomer(Customers customer);

        public Task<Customers> UpdateCustomer(long id, Customers customer);

        public Task DeleteCustomer(long id);

        public Task<IEnumerable<Customers>> GetAllCustomers();

        public Task<Customers> GetCustomerById(long id);

        public Task<IEnumerable<Customers>> GetAllCustomersByGender(int genderId);

        public Task<IEnumerable<Customers>> GetAllBlockCustomers();
    }
}
