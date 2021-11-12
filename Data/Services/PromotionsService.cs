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

        public async Task<Promos> UpdatePromo(long id, Promos promos)
        {
            _context.Update(id);
            await _context.SaveChangesAsync();

            return promos;
        }

        public async Task DeletePromo(long id)
        {
            var result = _context.Promos.FirstOrDefault(promos => promos.Id == id);
            _context.Promos.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Promos>> GetAllPromos()
        {
            var result = await _context.Promos.ToListAsync();
            return result;
        }

        public async Task<Promos> GetPromoById(long id)
        {
            var result = await _context.Promos.ToListAsync();
            return result.Find(x => x.Id == id);
        }

    }
}
