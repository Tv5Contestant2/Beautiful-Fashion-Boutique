using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ECommerce1.ViewModel
{
    public class HomeUserViewModel
    {
        public string Id { get; set; }
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

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "The Email Address is required")]
        [EmailAddress]
        public string SignInEmail { get; set; }

        [Required(ErrorMessage = "The Password is required")]
        [DataType(DataType.Password)]
        public string SignInPassword { get; set; }

        public bool isLogInError { get; set; }
        public bool isSignUpError { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<User> Users { get; set; } 

        #region Pagination 
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Users.Count() / (double)ItemPerPage));
        }
        public IEnumerable<User> PaginatedList()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return Users.OrderBy(b => b.Id).Skip(start).Take(ItemPerPage);
        }
        #endregion
    }
}