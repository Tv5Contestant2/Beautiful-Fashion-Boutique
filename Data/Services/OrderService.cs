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
        public bool AddToOrder(OrderDetails order)
        {
            _context.OrdersDetails.Add(order);
            _context.SaveChanges();

            return true;
        }

        public async Task<IEnumerable<OrderDetails>> GetAllOrders()
        {
            var result = await _context.OrdersDetails.ToListAsync();

            List<OrderDetails> orders = result.GroupBy(x => x.TransactionId)
                .Select(x => new OrderDetails
                {
                    TransactionId = x.First().TransactionId,
                    ItemSubTotal = x.Sum(x => x.ItemSubTotal),
                    OrderDate = x.First().OrderDate,
                    CustomerName = "Test Customer",
                    ModeOfPayment = "Pay On Delivery"
                }).ToList();

            return orders;
        }

        public async Task<IEnumerable<OrderDetails>> GetAllOrdersByUser()
        {
            var result = await _context.OrdersDetails.ToListAsync();

            List<OrderDetails> orders = result.GroupBy(x => x.TransactionId)
                .Select(x => new OrderDetails
                {
                    TransactionId = x.First().TransactionId,
                    ItemSubTotal = x.Sum(x => x.ItemSubTotal),
                    OrderDate = x.First().OrderDate
                }).ToList();

            return orders;
        }
    }
}
