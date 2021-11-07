using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AppDBContext _context;

        public ProductsService(AppDBContext context)
        {
            _context = context;
        }

        public Product InitializeProduct()
        {
            var _result = new Product()
            {
                Colors = _context.Colors.OrderBy(x => x.Id).ToList(),
                Genders = _context.Genders.ToList(),
                ProductCategories =  _context.ProductCategories.ToList(),
                Sizes = _context.Sizes.OrderBy(x => x.Id).ToList(),
                Statuses =  _context.Statuses.OrderBy(x => x.Id).ToList()
            };

            return _result;
        }

        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product UpdateProduct(long Id, Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(long id)
        {
            var result = _context.Products.Find(id);
            _context.Remove(result);

            _context.SaveChanges();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var result = await _context.Products.ToListAsync();
            return result;
        }

        public async Task<Product> GetProductById(long id)
        {
            var result = await _context.Products.ToListAsync();
            return result.Find(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsByGender(int genderId)
        {
            var result = await _context.Products.ToListAsync();
            return result.Where(x => x.GenderId == genderId).ToList();
        }
    }
}
