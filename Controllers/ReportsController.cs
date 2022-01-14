using ClosedXML.Excel;
using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ReportsController : Controller
    {
        private readonly ICommonServices _commonService;
        private readonly IReportService _service;
        private readonly IProductsService _productService;
        private readonly UserManager<User> _userManager;

        public ReportsController(IReportService service
            , ICommonServices commonService
            , IProductsService productService
            , UserManager<User> userManager)
        {
            _service = service;
            _commonService = commonService;
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewMonthlySalesReport(int page = 1)
        {
            var result = _service.GetMonthlySalesReport();

            var viewModel = new OrderViewModel
            {
                ItemPerPage = 10,
                Orders = result,
                CurrentPage = page
            };

            return View("MonthlySales", viewModel);
        }

        public async Task<IActionResult> ViewAnnualSalesReport(int page = 1)
        {
            var result = _service.GetAnnualSalesReport();

            var viewModel = new OrderViewModel
            {
                ItemPerPage = 10,
                Orders = result,
                CurrentPage = page
            };

            return View("AnnualSales", viewModel);
        }

        public async Task<IActionResult> ViewProductList(int page = 1)
        {
            var result = await _productService.GetAllProducts();

            var viewModel = new ProductViewModel
            {
                ItemPerPage = 10,
                Products = result,
                CurrentPage = page
            };

            return View("ProductList", viewModel);
        }

        public async Task GetMonthlySalesReport()
        {
            var result =_service.GetMonthlySalesReport();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(DateTime.Now.ToString("MMMM"));

                worksheet.Cell(1, 1).Value = "Monthly Sales Report";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = "As of " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                worksheet.Cell(2, 1).Style.Font.Italic = true;

                var currentRow = 4;
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Value = "Transaction ID";
                worksheet.Cell(currentRow, 2).Value = "Customer";
                worksheet.Cell(currentRow, 3).Value = "Order Date";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Status";
                worksheet.Cell(currentRow, 6).Value = "Delivery Status";
                worksheet.Cell(currentRow, 7).Value = "Expected Delivery Date";
                worksheet.Cell(currentRow, 8).Value = "Date Received";
                worksheet.ColumnWidth = 35;

                worksheet.Cell(currentRow, 1).Value = "Transaction ID";
                worksheet.Cell(currentRow, 2).Value = "Customer";
                worksheet.Cell(currentRow, 3).Value = "Order Date";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Status";
                worksheet.Cell(currentRow, 6).Value = "Delivery Status";
                worksheet.Cell(currentRow, 7).Value = "Expected Delivery Date";
                worksheet.Cell(currentRow, 8).Value = "Date Received";
                worksheet.ColumnWidth = 35;

                foreach (var item in result)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.TransactionId;
                    worksheet.Cell(currentRow, 2).Value = item.Customers.FirstName + ' ' + item.Customers.LastName;
                    worksheet.Cell(currentRow, 3).Value = item.OrderDate;
                    worksheet.Cell(currentRow, 4).Value = "Php " + item.Total;
                    worksheet.Cell(currentRow, 5).Value = item.OrderStatus.Title;
                    worksheet.Cell(currentRow, 6).Value = item.DeliveryStatus.Title;
                    worksheet.Cell(currentRow, 7).Value = item.ExpectedDeliveryDate;
                    worksheet.Cell(currentRow, 8).Value = item.ReceivedDate;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=MonthlySalesReport_" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + ".xls");
                Response.ContentType = "application/xls";
                await Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
        }

        public async Task GetAnnualSalesReport()
        {
            var result =_service.GetAnnualSalesReport();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(DateTime.Now.Year.ToString());

                worksheet.Cell(1, 1).Value = "Annual Sales Report";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = "As of " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                worksheet.Cell(2, 1).Style.Font.Italic = true;

                var currentRow = 4; 
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Value = "Transaction ID";
                worksheet.Cell(currentRow, 2).Value = "Customer";
                worksheet.Cell(currentRow, 3).Value = "Order Date";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Status";
                worksheet.Cell(currentRow, 6).Value = "Delivery Status";
                worksheet.Cell(currentRow, 7).Value = "Expected Delivery Date";
                worksheet.Cell(currentRow, 8).Value = "Date Received";
                worksheet.ColumnWidth = 35;

                foreach (var item in result)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.TransactionId;
                    worksheet.Cell(currentRow, 2).Value = item.Customers.FirstName + ' ' + item.Customers.LastName;
                    worksheet.Cell(currentRow, 3).Value = item.OrderDate;
                    worksheet.Cell(currentRow, 4).Value = "Php " + item.Total;
                    worksheet.Cell(currentRow, 5).Value = item.OrderStatus.Title;
                    worksheet.Cell(currentRow, 6).Value = item.DeliveryStatus.Title;
                    worksheet.Cell(currentRow, 7).Value = item.ExpectedDeliveryDate;
                    worksheet.Cell(currentRow, 8).Value = item.ReceivedDate;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=AnnualSalesReport_" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + ".xls");
                Response.ContentType = "application/xls";
                await Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
        }

        public async Task GetProductList()
        {
            var result = await _productService.GetAllProducts();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(DateTime.Now.Year.ToString());

                worksheet.Cell(1, 1).Value = "Product List";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = "As of " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                worksheet.Cell(2, 1).Style.Font.Italic = true;

                var currentRow = 4; 
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Value = "Product";
                worksheet.Cell(currentRow, 2).Value = "Category";
                worksheet.Cell(currentRow, 3).Value = "Price";
                worksheet.Cell(currentRow, 4).Value = "Stock Qty";
                worksheet.Cell(currentRow, 5).Value = "Critical Qty";
                worksheet.Cell(currentRow, 6).Value = "Stock Status";
                worksheet.ColumnWidth = 35;

                foreach (var item in result)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Name;
                    worksheet.Cell(currentRow, 2).Value = item.Category.Title;
                    worksheet.Cell(currentRow, 3).Value = "Php " + item.Price.ToString("#,##0.00");
                    worksheet.Cell(currentRow, 4).Value = item.StockQty;
                    worksheet.Cell(currentRow, 5).Value = item.CriticalQty;
                    worksheet.Cell(currentRow, 6).Value = item.InventoryStatus.Title;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=ProductList_" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + ".xls");
                Response.ContentType = "application/xls";
                await Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
        }

    }
}