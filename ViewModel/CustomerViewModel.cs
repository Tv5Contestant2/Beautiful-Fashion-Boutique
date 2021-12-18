using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.ViewModel
{
    public class CustomerViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Required(ErrorMessage = "The Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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

        public string Image { get; set; }

        public DateTime DateCreated { get; set; }

        #region Enumerables

        public IEnumerable<Gender> Genders { get; set; }

        #endregion Enumerables
    }
}