using System.ComponentModel.DataAnnotations;

namespace ECommerce1.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool isLogInError { get; set; }
    }
}
