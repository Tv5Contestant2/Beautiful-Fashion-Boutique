using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IReportService
    {
        public IEnumerable<Orders> GetMonthlySalesReport();

        public IEnumerable<Orders> GetAnnualSalesReport();
    }
}
