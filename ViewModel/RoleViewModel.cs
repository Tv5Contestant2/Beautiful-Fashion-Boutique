using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.ViewModel
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}