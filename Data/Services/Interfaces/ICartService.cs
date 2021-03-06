using ECommerce1.Models;
using ECommerce1.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICartService
    {
        public Task<Cart> GetCart(string userId);

        public Task<IEnumerable<CartDetails>> GetCartItems(string userId);

        public Task<IEnumerable<Wishlist>> GetWishlistItems(string userId);

        public bool CheckIfExistInWishlist(long productId, string userId);

        public Task<int> GetCartTotalQty(string userId);

        public Task<int> GetWishlistCount(string userId);

        public Task CreateCart(Cart cart);

        public Task UpdateCart(string userId, Cart cart);

        public void AddToCartItems(CartDetails cartDetails);

        public void AddToWishlist(Wishlist wishlist, CartDetails cartDetails);

        public void UpdateCartItems(CartDetails cartDetails);

        public Task RemoveFromCart(long id, string userId);

        public Task RemoveFromWishlist(long productId, string userId);

        public Task<CartDetails> GetCartItemsById(long id);

        public Task<CartDetails> GetCartItemsByProductId(long productId, int colorId, int sizeId, string userId);

        public Task<Wishlist> GetWishlistItemsByProductId(long productId, string userId);

        public Task EmptyCart(string userId);

    }
}
