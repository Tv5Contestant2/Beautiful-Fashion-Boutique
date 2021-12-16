using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IAdministratorService
    {
        public About GetAboutUs();

        public SocMed GetContactUs();

        public void CreateAboutUs(About about);

        public void UpdateAboutUs(About about);

        public void CreateContactUs(SocMed socMed);

        public void UpdateContactUs(SocMed socMed);
        public decimal GetProductSales();
        public int GetProductSold();
        public int GetPendingOrders();

    }
}
