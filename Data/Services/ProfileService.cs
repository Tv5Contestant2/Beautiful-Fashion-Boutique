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
                .Include(x => x.OrderStatus)
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

        public async Task<IEnumerable<OrderDetails>> GetCustomerReturns(string userId)
        {
            var result = await _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Orders)
                .Include(x => x.ReturnStatus)
                .Where(x => x.Orders.CustomersId == userId 
                         && x.IsReturned == true)
                .ToListAsync();

            return result;
        }
    }
}
