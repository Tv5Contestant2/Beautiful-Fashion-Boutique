using ClosedXML.Excel;
using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ReportsController : Controller
    {
        private readonly IReportService _service;
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public ReportsController(IReportService service
            , AppDBContext context
            , UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void GetMonthlySalesReport()
        {
            var orders = _service.GetMonthlySalesReport();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(DateTime.Now.ToString("MMMM"));
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Order Date";
                worksheet.Cell(currentRow, 2).Value = "Customer";
                worksheet.Cell(currentRow, 3).Value = "Delivery Status";
                worksheet.Cell(currentRow, 4).Value = "Expected Delivery Date";
                worksheet.Cell(currentRow, 5).Value = "Total";
                worksheet.ColumnWidth = 35;

                foreach (var item in orders)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.OrderDate;
                    worksheet.Cell(currentRow, 2).Value = item.Customers.FirstName + ' ' + item.Customers.LastName;
                    worksheet.Cell(currentRow, 3).Value = item.DeliveryStatus.Title;
                    worksheet.Cell(currentRow, 4).Value = item.ExpectedDeliveryDate;
                    worksheet.Cell(currentRow, 5).Value = item.Total;
                }
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=MonthlySales.xls");
                Response.ContentType = "application/xls";
                Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
        }
        public void GetAnnualSalesReport()
        {
            var orders = _service.GetAnnualSalesReport();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(DateTime.Now.ToString("YYYY"));
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Order Date";
                worksheet.Cell(currentRow, 2).Value = "Customer";
                worksheet.Cell(currentRow, 3).Value = "Delivery Status";
                worksheet.Cell(currentRow, 4).Value = "Expected Delivery Date";
                worksheet.Cell(currentRow, 5).Value = "Total";
                worksheet.ColumnWidth = 35;

                foreach (var item in orders)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.OrderDate;
                    worksheet.Cell(currentRow, 2).Value = item.Customers.FirstName + ' ' + item.Customers.LastName;
                    worksheet.Cell(currentRow, 3).Value = item.DeliveryStatus.Title;
                    worksheet.Cell(currentRow, 4).Value = item.ExpectedDeliveryDate;
                    worksheet.Cell(currentRow, 5).Value = item.Total;
                }
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=AnnualSales.xls");
                Response.ContentType = "application/xls";
                Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
        }

    }
}