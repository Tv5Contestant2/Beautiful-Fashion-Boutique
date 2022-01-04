using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.ViewModel
{
    public class RoleViewModel
    {
        public string Role { get; set; }
        public IEnumerable<Modules> Modules { get; set; }
    }
}
