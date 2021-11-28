using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class CartService : ICartService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;

        public CartService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public async Task<IEnumerable<Cart>> GetCartItems(string userId)
        {
            var result = await _context.Carts
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .Where(x => x.CustomersId == userId)
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
                    item.Product.Image = _commonServices.NoImage;
                }
            }

            return _cart;
        }

        public void AddToCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public async Task RemoveFromCart(long productId, string userId)
        {
            var result = _context.Carts.FirstOrDefault(x => x.ProductId == productId && x.CustomersId == userId);
            _context.Carts.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAllFromCart(long productId, string userId)
        {
            var result = _context.Carts.Where(x => x.ProductId == productId && x.CustomersId == userId).ToList();

            foreach (var item in result)
            {
                _context.Carts.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EmptyCart(string userId)
        {
            var result = _context.Carts.Where(x => x.CustomersId == userId).ToList();

            foreach (var item in result)
            {
                _context.Carts.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartItemsByProductId(long productId, string userId)
        {
            var result = await _context.Carts.ToListAsync();
            return result.Find(x => x.ProductId == productId && x.CustomersId == userId);
        }
    }
}