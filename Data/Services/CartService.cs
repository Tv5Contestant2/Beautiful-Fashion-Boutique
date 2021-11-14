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
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .ToListAsync();

            List<Cart> _cart = result
                .GroupBy(l => l.ProductId)
                .Select(cl => new Cart
                {
                    ProductId = cl.First().ProductId,
                    Quantity = cl.Sum(x => x.Quantity),
                    Product = cl.First().Product
                }).ToList();


            foreach (var item in _cart)
            {
                if (item.Product.ProductImages.Any())
                {
                    var _selectFirstImage = item.Product.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Product.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Product.Image = _noImage;
                }
            }

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

        public async Task EmptyCart()
        {
            var result = _context.Carts.ToList();

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

        private string _noImage = "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";
    }
}
