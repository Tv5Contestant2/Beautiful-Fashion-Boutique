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
            var result = await _context.Orders.ToListAsync();

            //List<Orders> orders = result.GroupBy(x => x.TransactionId)
            //    .Select(x => new Orders
            //    {
            //        TransactionId = x.First().TransactionId,
            //        Total = x.Sum(x => x.OrderDetails.ItemSubTotal),
            //        OrderDate = x.First().OrderDate,
            //        CustomerName = "Test Customer",
            //        ModeOfPayment = "Pay On Delivery"
            //    }).ToList();

            //return orders;
            return result;
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersByUser()
        {
            var result = await _context.Orders.ToListAsync();

            //List<Orders> orders = result.GroupBy(x => x.TransactionId)
            //    .Select(x => new Orders
            //    {
            //        TransactionId = x.First().TransactionId,
            //        ItemSubTotal = x.Sum(x => x.ItemSubTotal),
            //        OrderDate = x.First().OrderDate
            //    }).ToList();

            //return orders;
            return result;
        }
    }
}
