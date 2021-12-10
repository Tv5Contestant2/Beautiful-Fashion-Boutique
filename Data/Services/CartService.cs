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

        public async Task<int> GetWishlistCount(string userId)
        {
            var result = await _context.Wishlists
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result.Count();
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

        public async Task<IEnumerable<Wishlist>> GetWishlistItems(string userId)
        {
            var result = await _context.Wishlists
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            foreach (var item in result)
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

            return result;
        }

        public bool CheckIfExistInWishlist(long id, string userId)
        {
            var result = _context.Wishlists.Any(x => x.ProductId == id && x.CustomersId == userId);

            return result;
        }

        public async Task CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCart(string userId, Cart cart)
        {
            var result = _context.Carts.Where(x => x.CustomersId == userId).FirstOrDefault();

            if (result != null)
            {
                result.Instructions = cart.Instructions;
                result.ShippingFirstName = cart.ShippingFirstName;
                result.ShippingLastName = cart.ShippingLastName;
                result.ShippingBlock = cart.ShippingBlock;
                result.ShippingLot = cart.ShippingLot;
                result.ShippingBarangay = cart.ShippingBarangay;
                result.ShippingCity = cart.ShippingCity;
                result.ShippingContactNumber = cart.ShippingContactNumber;
                result.ShippingEmail = cart.ShippingEmail;
                result.IsBillingSame = cart.IsBillingSame;
                result.BillingFirstName = cart.IsBillingSame ? cart.ShippingFirstName : cart.BillingFirstName;
                result.BillingLastName = cart.IsBillingSame ? cart.ShippingLastName : cart.BillingLastName;
                result.BillingBlock = cart.IsBillingSame ? cart.ShippingBlock : cart.BillingBlock;
                result.BillingLot = cart.IsBillingSame ? cart.ShippingLot : cart.BillingLot;
                result.BillingBarangay = cart.IsBillingSame ? cart.ShippingBarangay : cart.BillingBarangay;
                result.BillingCity = cart.IsBillingSame ? cart.ShippingCity : cart.BillingCity;
                result.BillingContactNumber = cart.IsBillingSame ? cart.ShippingContactNumber : cart.BillingContactNumber;
                result.BillingEmail = cart.IsBillingSame ? cart.ShippingEmail : cart.BillingEmail;

                result.IsCashOnDelivery = cart.IsCashOnDelivery;
                result.IsPayMaya = cart.IsPayMaya;

                result.IsPickup = cart.IsPickup;
                result.IsDelivery = cart.IsDelivery;

                result.Total = cart.Total;

                _context.Carts.Update(result);
                await _context.SaveChangesAsync();
            }
        }

        public void AddToCartItems(CartDetails cartDetails)
        {
            _context.CartDetails.Add(cartDetails);
            _context.SaveChanges();
        }

        public void AddToWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
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

            if (!_context.CartDetails.Any(x => x.CustomersId == userId))
            {
                var cart = _context.Carts.FirstOrDefault(x => x.CustomersId == userId);
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
            }
        }
        
        public async Task RemoveFromWishlist(long productId, string userId)
        {
            var result = _context.Wishlists.FirstOrDefault(x => x.ProductId == productId && x.CustomersId == userId);
            _context.Wishlists.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task EmptyCart(string userId)
        {
            var cartDetails = _context.CartDetails.Where(x => x.CustomersId == userId).ToList();
            var cart = _context.Carts.FirstOrDefault(x => x.CustomersId == userId); 
            foreach (var item in cartDetails)
            {
                _context.CartDetails.Remove(item);
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<CartDetails> GetCartItemsByProductId(long productId, string userId)
        {
            var result = await _context.CartDetails.ToListAsync();
            return result.Find(x => x.ProductId == productId && x.CustomersId == userId);
        }

        public async Task<Wishlist> GetWishlistItemsByProductId(long productId, string userId)
        {
            var result = await _context.Wishlists.ToListAsync();
            return result.Find(x => x.ProductId == productId && x.CustomersId == userId);
        }
    }
}