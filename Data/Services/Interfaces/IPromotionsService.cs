using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IPromotionsService
    {
        public void CreatePromo(Promos promos);

        public Task<Promos> UpdatePromo(long id, Promos promos);

        public Task DeletePromo(long id);

        public Task<IEnumerable<Promos>> GetAllPromos();

        public Task<Promos> GetPromoById(long id);
    }
}
