using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IPromotionsService
    {
        public Task<IEnumerable<Promos>> GetAllPromos();
        public Promos GetPromoById(long Id);
        public void CreatePromo (Promos promos);
        public Promos UpdatePromo(long Id, Promos promos);
        public Task<bool> DeletePromo(long Id);
    }
}
