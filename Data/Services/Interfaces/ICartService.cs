using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICartService
    {
        Cart InitializeCart();

        public Task<IEnumerable<Cart>> GetCacheCartItems();

        public void AddToCart(Cart cart);

        public Task<Cart> UpdateCart(long id, Cart cart);

        public Task DeleteCart(long id);

        public Task<IEnumerable<Cart>> GetAllCarts();

        public Task<Cart> GetCartById(long id);

        public Task<IEnumerable<Cart>> GetAllCartsByGender(int genderId);
    }
}
