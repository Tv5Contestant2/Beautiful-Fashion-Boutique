using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce1.Models
{
    public class Messages
    {
        [Key]
        public int MessageID { get; set; }
        public int EmployeeID { get; set; }
        public string MessageTitle { get; set; }
        public int MessageRead { get; set; }
        public int MessageTrash { get; set; }
    }
}
