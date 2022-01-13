using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.ViewModel
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }

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

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAutoGeneratePassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        public IEnumerable<IdentityRole> RoleList { get; set; }
    }
}