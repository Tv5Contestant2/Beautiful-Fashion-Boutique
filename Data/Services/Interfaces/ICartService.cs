using ECommerce1.Models;
using ECommerce1.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICartService
    {
        #region Cart
        public Task<CartViewModel> InitializeCart(string userId);

        public Task<Cart> GetCart(string userId);

        public Task<IEnumerable<Cart>> GetCartItems(string userId);

        public Task<bool> IsProductInCart(string userId, CartDetails cartDetails);

        public Task<long> CreateCart(string userId);

        public Task AddToCartItems(long cartId, CartDetails cartDetails);

        public Task UpdateCartItems(CartDetails cartDetails);

        public Task RemoveFromCart(long cartDetailId);

        public Task IncreaseQuantity(long cartDetailId, int quantity);

        public Task DecreaseQuantity(long cartDetailId);

        public Task<int> GetCartCount(string userId);

        #endregion Cart


        #region Wishlist
        public Task<WishlistViewModel> InitializeWishlist(string userId);

        public Task<IEnumerable<Wishlist>> GetWishlistItems(string userId);

        public bool CheckIfExistInWishlist(string userId, long productId);

        public Task AddToWishlist(string userId, long cartDetailId);

        public Task<int> GetWishlistCount(string userId);


        #endregion Wishlist

        //public Task<int> GetCartTotalQty(string userId);

        //public Task<int> GetWishlistCount(string userId);

        //public Task CreateCart(Cart cart);

        //public Task UpdateCart(string userId, Cart cart);

        //public void AddToCartItems(CartDetails cartDetails);

        //public void UpdateCartItems(CartDetails cartDetails);

        //public Task RemoveFromWishlist(long productId, string userId);

        //public Task<CartDetails> GetCartItemsByProductId(long productId, int colorId, int sizeId, string userId);

        //public Task<Wishlist> GetWishlistItemsByProductId(long productId, string userId);

        //public Task EmptyCart(string userId);

    }
}
