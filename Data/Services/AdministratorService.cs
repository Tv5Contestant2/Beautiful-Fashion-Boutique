using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDBContext _context;

        public AdministratorService(AppDBContext context)
        {
            _context = context;
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
            var result = _context.Orders.Count(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Pending);
            return result != 0 ? result : 0;
        }
    }
}
