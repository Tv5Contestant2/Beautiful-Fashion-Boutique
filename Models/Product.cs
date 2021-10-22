using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public int ColorId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        public bool IsFeaturedProduct { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public long StockQty { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int ProductCategoryId { get; set; }

        public int SizeId { get; set; }

        #region Enumerables
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<ProductVariant> ProductVariants { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        #endregion Enumerables

    }
}
