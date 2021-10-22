using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Employees
    {
        [Key]
        public int EmployeeID { get; set; }

        [Display(Name = "Email")]
        public string EmployeeEmail { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string EmployeePass { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string EmployeeFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string EmployeeLastName { get; set; }

        [Display(Name = "Birthday")]
        public DateTime EmployeeBirthday { get; set; }

        [Display(Name = "Block")]
        public string EmployeeBlock { get; set; }

        [Display(Name = "Lot")]
        public string EmployeeLot { get; set; }

        [Display(Name = "City")]
        public string EmployeeCity { get; set; }

        [Display(Name = "Barangay")]
        public string EmployeeBaranggay { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public DateTime EmployeeUserCreation { get; set; }
        public DateTime DateCreated { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
        public int RoleID { get; set; }
        public int SecurityID { get; set; }

    }
}
