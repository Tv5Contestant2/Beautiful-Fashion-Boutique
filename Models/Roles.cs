using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string Role { get; set; }

    }
}
