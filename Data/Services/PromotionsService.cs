using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce1.Data.Enums;

namespace ECommerce1.Data.Services
{
    public class PromotionsService : IPromotionsService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;
        private bool _isResult;
        private string _resultMessage;

        public PromotionsService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public async Task<Promos> InitializePromo()
        {
            var _result = new Promos
            {
                ProductCategories = await _context.ProductCategories.OrderBy(x => x.Title).ToListAsync(),
                Genders = await _context.Genders.OrderBy(x => x.Title).ToListAsync(),
                Statuses = await _context.Statuses.ToListAsync(),
                Image = _commonServices.NoImage,
                StatusId = (int)StatusEnum.Active
            };

            return _result;
        }

        public async Task<Promos> InitializePromo(Promos model)
        {
            model.ProductCategories = await _context.ProductCategories.OrderBy(x => x.Title).ToListAsync();
            model.Genders = await _context.Genders.OrderBy(x => x.Title).ToListAsync();
            model.Statuses = await _context.Statuses.ToListAsync();
            model.Image = _commonServices.NoImage;
            model.StatusId = (int)StatusEnum.Active;

            return model;
        }

        public async Task<string> GetProductCategoryTitle(int id)
        {
            string value = string.Empty;
            var _result = await _context.ProductCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (_result != null) value = _result.Title;
            return value;
        }

        public async Task<string> GetGenderTitle(int id)
        {
            string value = string.Empty;
            var _result = await _context.Genders.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (_result != null) value = _result.Title;
            return value;
        }

        public async Task<(bool, string)> CreatePromo(Promos model)
        {
            _isResult = true;
            _resultMessage = string.Empty;
            try
            {
                await _context.Promos.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _isResult = false;
                _resultMessage = ex.ToString();
            }

            return (_isResult, _resultMessage);
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
            var result = await _context.Promos
                .Include(x => x.Status)
                .ToListAsync();
            return result;
        }

        public async Task<Promos> GetPromoById(long id)
        {
            var result = await _context.Promos.ToListAsync();
            return result.Find(x => x.Id == id);
        }
    }
}