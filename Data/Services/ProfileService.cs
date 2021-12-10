using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDBContext _context;

        public ProfileService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetCustomerOrders(string userId)
        {
            var result = await _context.Orders
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Wishlist>> GetCustomerWishlist(string userId)
        {
            var result = await _context.Wishlists
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CustomersBillingAddress>> GetCustomerBillingAddresses(string userId)
        {
            var result = await _context.CustomersBillingAddress
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CustomersShippingAddress>> GetCustomerShippingAddresses(string userId)
        {
            var result = await _context.CustomersShippingAddress
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }
    }
}
