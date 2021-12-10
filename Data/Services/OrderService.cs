using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _context;

        public OrderService(AppDBContext context)
        {
            _context = context;
        }
        public bool AddToOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return true;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            var result = await _context.Orders
                .Include(x => x.Customers)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersByUser(string userId)
        {
            var result = await _context.Orders
                .Include(x => x.Customers)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<OrderDetails>> GetOrderDetailsById(string transactionId)
        {
            var result = await _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Where(x => x.TransactionId.ToString() == transactionId)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetCustomerOrderCount(string userId)
        {
            var result = await _context.Orders
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result.Count();
        }
    }
}
