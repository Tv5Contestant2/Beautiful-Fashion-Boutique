using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<IEnumerable<Orders>> GetCustomerOrders(string userId);
        public Task<IEnumerable<OrderDetails>> GetCustomerReturns(string userId);
        public Task<IEnumerable<Wishlist>> GetCustomerWishlist(string userId);

    }
}
