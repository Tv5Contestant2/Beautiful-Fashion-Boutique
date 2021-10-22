using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class PromotionsService : IPromotionsService
    {
        private readonly AppDBContext _context;
        public PromotionsService(AppDBContext context)
        {
            _context = context;
        }

        public void CreatePromo(Promos promos)
        {
            _context.Promos.Add(promos);
            _context.SaveChanges();
        }

        public Task<bool> DeletePromo(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Promos>> GetAllPromos()
        {
            var result = await _context.Promos.ToListAsync();

            return result;
        }

        public Promos GetPromoById(long Id)
        {
            throw new NotImplementedException();
        }

        public Promos UpdatePromo(long Id, Promos promos)
        {
            throw new NotImplementedException();
        }

    }
}
