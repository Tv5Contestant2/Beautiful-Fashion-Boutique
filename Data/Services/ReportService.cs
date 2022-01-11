using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;

        public ReportService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public IEnumerable<Orders> GetMonthlySalesReport()
        {
            var result = _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .Where(x => x.OrderDate.Month == DateTime.Now.Month 
                    && x.OrderDate.Year == DateTime.Now.Year)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return result;
        }

        public IEnumerable<Orders> GetAnnualSalesReport()
        {
            var result = _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .Where(x => x.OrderDate.Year == DateTime.Now.Year)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return result;
        }
    }
}
