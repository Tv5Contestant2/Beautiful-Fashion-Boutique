using ECommerce1.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECommerce1.ViewModel
{
    public class PromoViewModel
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

        public IEnumerable<Promos> Promos { get; set; }

        #region Pagination 
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Promos.Count() / (double)ItemPerPage));
        }
        public IEnumerable<Promos> PaginatedList()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return Promos.OrderBy(b => b.Id).Skip(start).Take(ItemPerPage);
        }
        #endregion

    }
}
