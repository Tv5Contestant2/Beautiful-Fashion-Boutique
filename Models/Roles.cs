using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string EmployeeRole { get; set; }

    }
}
