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

        public Task<int> GetCartTotalQty(string userId);

        public Task CreateCart(Cart cart);

        public Task UpdateCart(string userId, Cart cart);

        public void AddToCartItems(CartDetails cartDetails);

        public void UpdateCartItems(CartDetails cartDetails);

        public Task RemoveFromCart(long productId, string userId);

        public Task<CartDetails> GetCartItemsByProductId(long productId, string userId);

        public Task EmptyCart(string userId);

    }
}
