using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonService;

        public AdministratorService(AppDBContext context
            , ICommonServices commonService)
        {
            _context = context;
            _commonService = commonService;
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

        public decimal GetProductSales()
        {
            var result = _context.Orders.Sum(x => x.Total + x.TaxAmount + x.ShippingFee);
            return result != 0 ? result : 0;
        }

        public int GetProductSold()
        {
            var result = _context.OrdersDetails.Sum(x => x.Quantity);
            return result != 0 ? result : 0;
        }

        public int GetPendingOrders()
        {
            var result = _context.Orders.Count(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Pending 
                            && x.OrderStatusId != (int)OrderStatusEnum.Cancelled);
            return result != 0 ? result : 0;
        }

        public IEnumerable<Orders> GetRecentOrders()
        {
            return _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .OrderByDescending(x => x.OrderDate)
                .Take(20).ToList();
        }

        public IEnumerable<Orders> GetRecentDeliveries()
        {
            return _context.Orders
                .Include(x => x.Customers)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Shipped)
                .OrderByDescending(x => x.OrderDate)
                .Take(20).ToList();
        }

        public IEnumerable<Message> GetRecentMessages()
        {
            return _context.Messages
                .Include(x => x.Sender)
                .OrderByDescending(x => x.DateSent)
                .Take(20).ToList();
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
                    item.Image = _commonService.NoImage;
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
    }
}
