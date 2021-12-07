using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
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
        private readonly ICommonServices _commonServices;

        public CartService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public async Task<Cart> GetCart(string userId)
        {
            var result = await _context.Carts
                .Include(x => x.Customers)
                .Where(x => x.CustomersId == userId)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> GetCartTotalQty(string userId)
        {
            var count = 0;
            var result = await _context.CartDetails
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            if (result.Count() > 0)
                count = result.Sum(x => x.Quantity);

            return count == 0 ? 0 : count;
        }

        public async Task<IEnumerable<CartDetails>> GetCartItems(string userId)
        {
            var result = await _context.CartDetails
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            List<CartDetails> _cart = result
                .GroupBy(l => l.ProductId)
                .Select(cl => new CartDetails
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

        public async Task CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void AddToCartItems(CartDetails cartDetails)
        {
            _context.CartDetails.Add(cartDetails);
            _context.SaveChanges();
        }

        public void UpdateCartItems(CartDetails cartDetails)
        {
            _context.CartDetails.Update(cartDetails);
            _context.SaveChanges();
        }
        
        public async Task RemoveFromCart(long productId, string userId)
        {
            var result = _context.CartDetails.FirstOrDefault(x => x.ProductId == productId && x.CustomersId == userId);
            _context.CartDetails.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task EmptyCart(string userId)
        {
            var result = _context.CartDetails.Where(x => x.CustomersId == userId).ToList();

            foreach (var item in result)
            {
                _context.CartDetails.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CartDetails> GetCartItemsByProductId(long productId, string userId)
        {
            var result = await _context.CartDetails.ToListAsync();
            return result.Find(x => x.ProductId == productId && x.CustomersId == userId);
        }
    }
}