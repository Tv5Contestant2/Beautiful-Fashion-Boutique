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
    }
}
