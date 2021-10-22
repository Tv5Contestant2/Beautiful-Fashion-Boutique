using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Promos
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        [Display(Name = "Promo Category")]
        public string PromoCategory { get; set; }

        [Display(Name = "Sale Percentage")]
        public int SalePercentage { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

    }
}
