using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce1.ViewModel
{
    public class StoreFrontViewModel
    {
        public int CartCount { get; set; }
        public int CurrentPage { get; set; }
        public string CustomersId { get; set; }
        public string FacebookLink { get; set; }
        public int ItemPerPage { get; set; }
        public string PageDescription { get; set; }
        public string PageTitle { get; set; }
        public int WishlistCount { get; set; }

        public CartDetails CartDetails { get; set; }
        public FilterViewModel Filter { get; set; }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }

        #region Pagination 
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Products.Count() / (double)ItemPerPage));
        }
        public IEnumerable<Product> PaginatedList()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return Products.OrderBy(b => b.Id).Skip(start).Take(ItemPerPage);
        }
        #endregion
    }
}
