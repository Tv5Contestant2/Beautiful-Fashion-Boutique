using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICustomersService
    {
        CustomerViewModel InitializeCustomer();

        Task<(bool, IEnumerable<IdentityError>)> CreateCustomer(CustomerViewModel model);

        Task UpdateCustomer(CustomerViewModel model);

        Task<User> GetCustomerById(string id);

        Task DeleteCustomer(string id);

        Task<IEnumerable<Gender>> GetGenders();

        Task UpdateCustomer(User model);

        Task<IEnumerable<User>> GetAllCustomers();

        Task<IEnumerable<User>> GetAllCustomersByGender(int genderId);

        Task<IEnumerable<User>> GetAllArchivedCustomers();

        Task<IEnumerable<User>> GetAllBlockCustomers();

        Task<IEnumerable<User>> GetAllDeletedCustomers();

        void InitializeCustomer(CustomerViewModel model);
    }
}