using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Promos
    {
        public long Id { get; set; }

        public DateTime DateCreated { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Promo Category")]
        public string PromoCategory { get; set; }

        public bool IsByCategory { get; set; }

        public bool IsByGender { get; set; }

        [Display(Name = "Sale Percentage")]
        [Range(1, 100)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePercentage { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? GenderId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        #region One to One

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual Status Status { get; set; }

        #endregion One to One

        #region One to Many

        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }

        public virtual IEnumerable<Gender> Genders { get; set; }

        public virtual IEnumerable<Status> Statuses { get; set; }

        #endregion One to Many
    }
}