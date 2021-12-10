using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<IEnumerable<Orders>> GetCustomerOrders(string userId);
        public Task<IEnumerable<Returns>> GetCustomerReturns(string userId);
        public Task<IEnumerable<Wishlist>> GetCustomerWishlist(string userId);
        public Task<IEnumerable<CustomersBillingAddress>> GetCustomerBillingAddresses(string userId);
        public Task<IEnumerable<CustomersShippingAddress>> GetCustomerShippingAddresses(string userId);

    }
}
