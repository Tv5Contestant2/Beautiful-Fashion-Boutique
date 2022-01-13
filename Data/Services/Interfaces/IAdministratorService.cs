using ECommerce1.Models;
using ECommerce1.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IAdministratorService
    {
        public string GetStoreLogo();

        public string GetHeroVideo();

        public About GetAboutUs();

        public SocMed GetContactUs();

        public string GetFacebookLink();

        public void CreateAboutUs(About about);

        public void UpdateAboutUs(About about);

        public void CreateContactUs(SocMed socMed);

        public void UpdateContactUs(SocMed socMed);

        public void UpdateSettings(SettingsViewModel settings);

        public decimal GetProductSales();

        public long GetProductSold();

        public long GetPendingOrders();

        public IEnumerable<Orders> GetRecentOrders();

        public IEnumerable<Orders> GetRecentDeliveries();

        public IEnumerable<Message> GetRecentMessages();

        public Task<IEnumerable<OrderDetails>> GetTopSelling();

        public Task<IEnumerable<Product>> GetOutOfStock();

        public Task<IEnumerable<Product>> GetCritical();

    }
}
