using Microsoft.AspNetCore.Identity;

namespace ECommerce1.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsCustomer { get; set; }
        public bool? IsEmployee { get; set; }
        public string LastName { get; set; }
    }
}