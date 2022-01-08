using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Messages = new HashSet<Message>();
        }

        public string FirstName { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsCustomer { get; set; }

        public bool? IsEmployee { get; set; }

        public bool? IsFirstTimeLogin { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        [Display(Name = "Block")]
        public string AddressBlock { get; set; }

        [Display(Name = "Lot")]
        public string AddressLot { get; set; }

        [Display(Name = "City")]
        public string AddressCity { get; set; }

        [Display(Name = "Barangay")]
        public string AddressBaranggay { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Image { get; set; }

        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Blocked")]
        public bool? IsBlock { get; set; }

        [Display(Name = "Archived")]
        public bool? IsArchived { get; set; }

        [Display(Name = "Last Logged In")]
        public DateTime LastLoggedIn { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}