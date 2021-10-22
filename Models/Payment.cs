using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public int EmployeeID { get; set; }
        public int CustomerID { get; set; }
        public int MethodID { get; set; }
        public int TransactionID { get; set; }

    }
}
