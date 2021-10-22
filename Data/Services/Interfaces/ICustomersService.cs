using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICustomersService
    {
        public Task<IEnumerable<Customers>> GetAllCustomers();
        public Customers GetCustomerById(long Id);
        public void CreateCustomer(Customers customers);
        public Customers UpdateCustomer(long Id, Customers customers);
        public Task<bool> DeleteCustomer(long Id);
        Customers InitializeCustomer();
    }
}
