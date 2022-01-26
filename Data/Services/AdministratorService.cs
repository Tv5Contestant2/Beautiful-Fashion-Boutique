using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;

        public AdministratorService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public string GetStoreLogo() 
        {
            var result = _context.Settings.Select(x => x.StoreLogo).First();
            return result;
        }

        public string GetHero() 
        {
            var result = _context.Settings.Select(x => x.StoreBanner).First();
            return result;
        }

        public string GetEmailLogo() 
        {
            var result = _context.Settings.Select(x => x.EmailLogo).First();
            return result;
        }

        public About GetAboutUs() 
        {
            var result = _context.AboutUs.OrderByDescending(x => x.Id).FirstOrDefault();
            return result;
        }

        public SocMed GetContactUs()
        {
            var result = _context.Socials.OrderByDescending(x => x.Id).FirstOrDefault();
            return result;
        }

        public string GetFacebookLink()
        {
            var result = _context.Socials.OrderByDescending(x => x.Id).FirstOrDefault();
            return result != null ? result.Facebook : "";
        }

        public void CreateAboutUs(About about)
        {
            _context.AboutUs.Add(about);
            _context.SaveChanges();
        }

        public void CreateContactUs(SocMed socMed)
        {
            _context.Socials.Add(socMed);
            _context.SaveChanges();
        }

        public void UpdateAboutUs(About about)
        {
            _context.AboutUs.Update(about);
            _context.SaveChanges();
        }

        public void UpdateContactUs(SocMed socMed)
        {
            _context.Socials.Update(socMed);
            _context.SaveChanges();
        }

        public void UpdateSettings(SettingsViewModel settings)
        {
            var result = _context.Settings.FirstOrDefault();

            if (settings.StoreLogo.Contains("data:image/jpeg"))
            {
                var _logo = settings.StoreLogo.Split(",");
                result.StoreLogo = _logo[1]; //Get Base64String only.
            }

            if (settings.StoreBanner.Contains("data:image/jpeg"))
            {
                var _hero = settings.StoreBanner.Split(",");
                result.StoreBanner = _hero[1]; //Get Base64String only.
            }

            result.EmailLogo = settings.EmailLogo;

            _context.Settings.Update(result);
            _context.SaveChanges();
        }

        public async Task<decimal> GetProductSales()
        {
            var result = await _context.Orders
                .Where(x => x.OrderDate.Month == DateTime.Now.Month && x.OrderDate.Year == DateTime.Now.Year)
                .ToListAsync();

            return result != null ? result.Sum(x => x.Total + x.TaxAmount + x.ShippingFee) : 0;
        }

        public async Task<long> GetProductSold()
        {
            var result = await _context.OrdersDetails
                .Where(x => x.Orders.OrderDate.Month == DateTime.Now.Month && x.Orders.OrderDate.Year == DateTime.Now.Year)
                .ToListAsync();

            return result != null ? result.Sum(x => x.Quantity) : 0;
        }

        public async Task<long> GetPendingOrders()
        {
            var result = await _context.Orders.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Pending 
                            && x.OrderStatusId != (int)OrderStatusEnum.Cancelled).ToListAsync();
            return result != null ? result.Count : 0;
        }

        public async Task<IEnumerable<Orders>> GetRecentOrders()
        {
            return await _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .OrderByDescending(x => x.OrderDate)
                .Take(20).ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetRecentDeliveries()
        {
            return await _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Shipped)
                .OrderByDescending(x => x.OrderDate)
                .Take(20).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetRecentMessages()
        {
            return await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.Sender.FirstName != "Administrator")
                .OrderByDescending(x => x.DateSent)
                .Take(20).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetails>> GetTopSelling()
        {
            var result = await _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Product)
                    .ThenInclude(x => x.InventoryStatus)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .ToListAsync();


            List<OrderDetails> orders = result
                .GroupBy(l => l.ProductId)
                .Select(cl => new OrderDetails
                {
                    ProductId = cl.First().ProductId,
                    Quantity = cl.Sum(x => x.Quantity),
                    Product = cl.First().Product
                }).ToList();

            foreach (var item in result)
            {
                if (item.Product.ProductImages.Any())
                {
                    var _selectFirstImage = item.Product.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Product.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Product.Image = _commonServices.NoImage;
                }
            }

            return orders.OrderByDescending(x => x.Quantity)
                         .Take(10).ToList();
        }

        public async Task<IEnumerable<Product>> GetOutOfStock()
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.InventoryStatus)
                .Include(x => x.Category)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.Where(x => x.StockStatusId == (int)StockStatusEnum.OutOfStock).ToList();
        }

        public async Task<IEnumerable<Product>> GetCritical()
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.InventoryStatus)
                .Include(x => x.Category)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.Where(x => x.StockStatusId == (int)StockStatusEnum.Critical).ToList();
        }
    }
}
