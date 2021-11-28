﻿using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICartService
    {
        public Task<IEnumerable<Cart>> GetCartItems(string userId);

        public void AddToCart(Cart cart);

        public Task RemoveFromCart(long productId, string userId);

        public Task RemoveAllFromCart(long productId, string userId);

        public Task<Cart> GetCartItemsByProductId(long productId, string userId);

        public Task EmptyCart(string userId);

    }
}
