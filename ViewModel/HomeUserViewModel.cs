using System.ComponentModel.DataAnnotations;

namespace ECommerce1.ViewModel
{
    public class HomeUserViewModel
    {
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
    }
}