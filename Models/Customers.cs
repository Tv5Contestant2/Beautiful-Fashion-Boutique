using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }

        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Password")]
        public string CustomerPass { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string CustomerFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string CustomerLastName { get; set; }

        [Display(Name = "Birthday")]
        public DateTime CustomerBirthday { get; set; }

        [Display(Name = "Block")]
        public string CustomerBlock { get; set; }

        [Display(Name = "Lot")]
        public string CustomerLot { get; set; }

        [Display(Name = "City")]
        public string CustomerCity { get; set; }

        [Display(Name = "Barangay")]
        public string CustomerBaranggay { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public DateTime CustomerUserCreation { get; set; }
        public DateTime DateCreated { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
        public int MessageID { get; set; }
        public int UserID { get; set; }

        #region Enumerables
        public IEnumerable<Gender> Genders { get; set; }
        #endregion Enumerables
    }
}
