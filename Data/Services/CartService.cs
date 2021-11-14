using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
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

        public CartService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetCacheCartItems()
        {
            var result = await _context.Carts.ToListAsync();
           
            return result;
        }

        public void AddToCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        Task ICartService.DeleteCart(long id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Cart>> ICartService.GetAllCarts()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Cart>> ICartService.GetAllCartsByGender(int genderId)
        {
            throw new NotImplementedException();
        }

        Task<Cart> ICartService.GetCartById(long id)
        {
            throw new NotImplementedException();
        }

        Cart ICartService.InitializeCart()
        {
            throw new NotImplementedException();
        }

        Task<Cart> ICartService.UpdateCart(long id, Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}
