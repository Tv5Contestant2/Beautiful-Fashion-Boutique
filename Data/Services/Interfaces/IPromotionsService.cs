using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IPromotionsService
    {
        Task<(bool, string)> CreatePromo(Promos model);

        public Task<Promos> UpdatePromo(long id, Promos promos);

        public Task DeletePromo(long id);

        public Task<IEnumerable<Promos>> GetAllPromos();

        public Task<Promos> GetPromoById(long id);

        Task<Promos> InitializePromo(Promos model);

        Task<Promos> InitializePromo();

        Task<string> GetProductCategoryTitle(int id);

        Task<string> GetGenderTitle(int id);
    }
}