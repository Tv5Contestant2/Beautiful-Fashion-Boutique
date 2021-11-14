using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class CartService : ICartService
    {
        private readonly AppDBContext _context;

        public CartService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetCacheCartItems()
        {
            var result = await _context.Carts
                .Include(x => x.Product)
                .ToListAsync();

            var cart = result.GroupBy(x => x.ProductId);

            List<Cart> _cart = result
            .GroupBy(l => l.ProductId)
            .Select(cl => new Cart
            {
                ProductId = cl.First().ProductId,
                Quantity = cl.Sum(x => x.Quantity),
                Product = cl.First().Product
            }).ToList();

            return _cart;
        }

        public void AddToCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public async Task RemoveFromCart(long productId)
        {
            var result = _context.Carts.FirstOrDefault(cart => cart.ProductId == productId);
            _context.Carts.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAllFromCart(long productId)
        {
            var result = _context.Carts.Where(x => x.ProductId == productId).ToList();

            foreach (var item in result)
            { 
                _context.Carts.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartItemsByProductId(long productId)
        {
            var result = await _context.Carts.ToListAsync();
            return result.Find(x => x.ProductId == productId);
        }
    }
}
