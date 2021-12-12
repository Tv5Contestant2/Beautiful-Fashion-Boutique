using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ECommerce1.ViewModel
{
    public class ProductViewModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int ProductCategoryId { get; set; }

        public string Review { get; set; }
        public long ProductId { get; set; }
        public string CustomersId { get; set; }

        public CartDetails CartDetails { get; set; }
        public Wishlist Wishlists { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductReview> ProductReviews { get; set; }

        #region Pagination 
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
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
