using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
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

        public CartService(AppDBContext context
            , ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        #region Cart

        public async Task<CartViewModel> InitializeCart(string userId)
        {
            var cartViewModel = new CartViewModel();

            var cart = await GetCart(userId);
            var cartDetails = await GetCartItems(userId);

            if (cartDetails.Count() > 0)
            {
                cartViewModel = new CartViewModel()
                {
                    Cart = cart,
                    CartCount = await GetCartCount(userId),
                    CartDetails = cartDetails.Select(x => x.CartDetails),
                    Colors = _commonServices.GetColors(),
                    CustomersId = userId,
                    Sizes = _commonServices.GetSizes(),
                    WishlistCount = await GetWishlistCount(userId)
                };
            }

            return cartViewModel;
        }

        public async Task<Cart> GetCart(string userId)
        {
            var result = await _context.Carts
                .Include(x => x.Customers)
                .Where(x => x.CustomersId == userId)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Cart>> GetCartItems(string userId)
        {
            var result = await _context.Carts
                .Include(x => x.CartDetails)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.ProductImages)
                .Include(x => x.CartDetails)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.ProductVariants).ThenInclude(x => x.Color)
                .Include(x => x.CartDetails)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.ProductVariants).ThenInclude(x => x.Size)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            foreach (var item in result.Select(x => x.CartDetails))
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

        public async Task<bool> IsProductInCart(string userId, CartDetails cartDetails)
        {
            var result = await _context.Carts
                .Include(x => x.CartDetails)
                .ToListAsync();

            return result.Any(x => x.CartDetails.ProductId == cartDetails.ProductId
                                 && x.CartDetails.ColorId == cartDetails.ColorId
                                 && x.CartDetails.SizeId == cartDetails.SizeId
                                 && x.CustomersId == userId);
        }

        public async Task<long> CreateCart(string userId)
        {
            var _cart = new Cart();
            _cart.CustomersId = userId;

            _context.Carts.Add(_cart);
            await _context.SaveChangesAsync();

            return _cart.Id;
        }

        public async Task AddToCartItems(long cartId, CartDetails cartDetails)
        {
            var _cartDetails = new CartDetails()
            { 
                CartId = cartId,
                ProductId = cartDetails.ProductId,
                ColorId = cartDetails.ColorId,
                SizeId = cartDetails.SizeId,
                Quantity = cartDetails.Quantity                
            };

            _context.CartDetails.Add(_cartDetails);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItems(CartDetails cartDetails)
        {
            var result = _context.CartDetails
                .Where(x => x.ProductId == cartDetails.ProductId)
                .FirstOrDefault();

            await IncreaseQuantity(result.Id, cartDetails.Quantity);
        }

        public async Task RemoveFromCart(long cartDetailId)
        {
            var result = _context.CartDetails.FirstOrDefault(x => x.Id == cartDetailId);
            _context.CartDetails.Remove(result);

            await _context.SaveChangesAsync();

            if (!_context.CartDetails.Any(x => x.CartId == result.CartId))
            {
                var cart = _context.Carts.FirstOrDefault(x => x.Id == result.CartId);
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
            }
        }

        public async Task IncreaseQuantity(long cartDetailId, int quantity)
        {
            var result = _context.CartDetails
                .Where(x => x.Id == cartDetailId)
                .FirstOrDefault();

            result.Quantity += quantity;

            _context.CartDetails.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseQuantity(long cartDetailId)
        {
            var result = _context.CartDetails
                .Where(x => x.Id == cartDetailId)
                .FirstOrDefault();

            if (result.Quantity > 1)
            {
                result.Quantity -= 1;

                _context.CartDetails.Update(result);
                await _context.SaveChangesAsync();
            }
            else
                await RemoveFromCart(cartDetailId);
        }

        public async Task<int> GetCartCount(string userId)
        {
            var count = 0;
            var result = await _context.Carts
                    .Include(x => x.CartDetails)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            if (result.Count() > 0)
                count = result.Sum(x => x.CartDetails.Quantity);

            return count == 0 ? 0 : count;
        }

        #endregion Cart

        #region Wishlist

        public async Task<WishlistViewModel> InitializeWishlist(string userId)
        {
            var wishlistViewModel = new WishlistViewModel();
            var wishlist = await GetWishlistItems(userId);

            if (wishlist.Count() > 0)
            {
                wishlistViewModel = new WishlistViewModel()
                {
                    CartCount = await GetCartCount(userId),
                    Colors = _commonServices.GetColors(),
                    CustomersId = userId,
                    Sizes = _commonServices.GetSizes(),
                    Wishlists = wishlist,
                    WishlistCount = await GetWishlistCount(userId),
                };
            }

            return wishlistViewModel;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistItems(string userId)
        {
            var result = await _context.Wishlists
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductVariants).ThenInclude(x => x.Size)
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductVariants).ThenInclude(x => x.Color)
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            foreach (var item in result.Select(x => x.Product))
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            return result;
        }

        public bool CheckIfExistInWishlist(string userId, long productId)
        {
            return _context.Wishlists.Any(x => x.CustomersId == userId && x.ProductId == productId);
        }

        public async Task AddToWishlist(string userId, long cartDetailId)
        {
            var cartDetails = _context.CartDetails.FirstOrDefault(x => x.Id == cartDetailId);
            var _wishlist = new Wishlist()
            {
                CustomersId = userId,
                ProductId = cartDetails.ProductId,
                ColorId = cartDetails.ColorId,
                SizeId = cartDetails.SizeId,
            };

            _context.Wishlists.Add(_wishlist);
            await _context.SaveChangesAsync();

            await RemoveFromCart(cartDetails.Id);
        }

        public async Task<int> GetWishlistCount(string userId)
        {
            var result = await _context.Wishlists
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result.Count();
        }

        #endregion Wishlist
    }






    //        public async Task<IEnumerable<Wishlist>> GetWishlistItems(string userId)
    //        {
    //            var result = await _context.Wishlists
    //                .Include(x => x.Product)
    //                    .ThenInclude(x => x.ProductVariants)
    //                        .ThenInclude(x => x.Size)
    //                .Include(x => x.Product)
    //                    .ThenInclude(x => x.ProductVariants)
    //                        .ThenInclude(x => x.Color)
    //                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
    //                .Where(x => x.CustomersId == userId)
    //                .ToListAsync();

    //            foreach (var item in result)
    //            {
    //                if (item.Product.ProductImages.Any())
    //                {
    //                    var _selectFirstImage = item.Product.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
    //                    item.Product.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
    //                }
    //                else
    //                {
    //                    //No Image
    //                    item.Product.Image = _commonServices.NoImage;
    //                }
    //            }

    //            return result;
    //        }

    //        public async Task CreateCart(Cart cart)
    //        {
    //            _context.Carts.Add(cart);
    //            await _context.SaveChangesAsync();
    //        }

    //        public async Task UpdateCart(string userId, Cart cart)
    //        {
    //            var result = _context.Carts.Where(x => x.CustomersId == userId).FirstOrDefault();

    //            if (result != null)
    //            {
    //                result.Instructions = cart.Instructions;
    //                result.ShippingFirstName = cart.ShippingFirstName;
    //                result.ShippingLastName = cart.ShippingLastName;
    //                result.ShippingBlock = cart.ShippingBlock;
    //                result.ShippingLot = cart.ShippingLot;
    //                result.ShippingBarangay = cart.ShippingBarangay;
    //                result.ShippingCity = cart.ShippingCity;
    //                result.ShippingContactNumber = cart.ShippingContactNumber;
    //                result.ShippingEmail = cart.ShippingEmail;
    //                result.IsBillingSame = cart.IsBillingSame;
    //                result.BillingFirstName = cart.IsBillingSame ? cart.ShippingFirstName : cart.BillingFirstName;
    //                result.BillingLastName = cart.IsBillingSame ? cart.ShippingLastName : cart.BillingLastName;
    //                result.BillingBlock = cart.IsBillingSame ? cart.ShippingBlock : cart.BillingBlock;
    //                result.BillingLot = cart.IsBillingSame ? cart.ShippingLot : cart.BillingLot;
    //                result.BillingBarangay = cart.IsBillingSame ? cart.ShippingBarangay : cart.BillingBarangay;
    //                result.BillingCity = cart.IsBillingSame ? cart.ShippingCity : cart.BillingCity;
    //                result.BillingContactNumber = cart.IsBillingSame ? cart.ShippingContactNumber : cart.BillingContactNumber;
    //                result.BillingEmail = cart.IsBillingSame ? cart.ShippingEmail : cart.BillingEmail;

    //                result.IsCashOnDelivery = cart.IsCashOnDelivery;
    //                result.IsPayMaya = cart.IsPayMaya;

    //                result.IsPickup = cart.IsPickup;
    //                result.IsDelivery = cart.IsDelivery;

    //                result.Total = cart.Total;

    //                _context.Carts.Update(result);
    //                await _context.SaveChangesAsync();
    //            }
    //        }

    //        public void AddToCartItems(CartDetails cartDetails)
    //        {
    //            _context.CartDetails.Add(cartDetails);
    //            _context.SaveChanges();
    //        }
    //        public async Task RemoveFromWishlist(long productId, string userId)
    //        {
    //            var result = _context.Wishlists.FirstOrDefault(x => x.ProductId == productId && x.CustomersId == userId);
    //            _context.Wishlists.Remove(result);

    //            await _context.SaveChangesAsync();
    //        }

    //        public async Task EmptyCart(string userId)
    //        {
    //            var cartDetails = _context.CartDetails.Where(x => x.CustomersId == userId).ToList();
    //            var cart = _context.Carts.FirstOrDefault(x => x.CustomersId == userId); 
    //            foreach (var item in cartDetails)
    //            {
    //                _context.CartDetails.Remove(item);
    //            }

    //            _context.Carts.Remove(cart);
    //            await _context.SaveChangesAsync();
    //        }



    //        public async Task<Wishlist> GetWishlistItemsByProductId(long productId, string userId)
    //        {
    //            var result = await _context.Wishlists
    //                //.Include(x => x.Product)
    //                //    .ThenInclude(x => x.ProductVariants)
    //                //        .ThenInclude(x => x.Size)
    //                //.Include(x => x.Product)
    //                //    .ThenInclude(x => x.ProductVariants)
    //                //        .ThenInclude(x => x.Color)
    //                .ToListAsync();
    //            return result.Find(x => x.ProductId == productId
    //                                 //&& x.ColorId == colorId
    //                                 //&& x.SizeId == sizeId
    //                                 && x.CustomersId == userId);
    //        }
    //    }
}