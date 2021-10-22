using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Security
    {
        [Key]
        public int SecurityID { get; set; }
        public string EmployeeSecurityQuestion { get; set; }
        public string EmployeeSecurityAnswer { get; set; }
    }
}
