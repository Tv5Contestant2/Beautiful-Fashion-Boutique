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

        public string Description { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int ColorId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public int StockStatusId { get; set; }

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

        [Required]
        [Display(Name = "Critical Quantity")]
        public int CriticalQty { get; set; }

        public int SizeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Is on sale?")]
        public bool IsSale { get; set; }

        [Display(Name = "Price on sale")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PriceOnSale { get; set; }

        #region Enumerables

        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<ProductVariant> ProductVariants { get; set; }
        public IEnumerable<ProductReview> ProductReviews { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<StockStatus> StockStatuses { get; set; }
        public StockStatus InventoryStatus { get; set; }
        public ProductCategory Category { get; set; }

        #endregion Enumerables

        //Added temp
        [NotMapped]
        public string ProductVariantJSON { get; set; }

        [NotMapped]
        public string ProductImageJSON { get; set; }
    }
}