using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICartService
    {
        public Task<IEnumerable<Cart>> GetCacheCartItems();

        public void AddToCart(Cart cart);

        public Task RemoveFromCart(long productId);

        public Task RemoveAllFromCart(long productId);

        public Task<Cart> GetCartItemsByProductId(long productId);

        public Task EmptyCart();

    }
}
