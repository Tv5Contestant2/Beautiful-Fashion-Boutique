using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly AppDBContext _context;

        public ProductCategoriesService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategories()
        {
            var result = await _context.ProductCategories.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Size>> GetSizesPerCategory(int categoryId)
        {
            var result = new List<Size>();
            if (categoryId != 0)
            {
                result = await _context.Sizes.Where(x => x.CategoryId == categoryId).OrderByDescending(x => x.CategoryId).ToListAsync();
            }
            else {
                result = await _context.Sizes.OrderByDescending(x => x.CategoryId).ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<Color>> GetColors()
        {
            return await _context.Colors.OrderBy(x => x.Title).ToListAsync();
        }

        public async Task<IEnumerable<Size>> GetSizes()
        {
            return await _context.Sizes.OrderBy(x => x.Title).ToListAsync();
        }
    }
}
