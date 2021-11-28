using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public int TransactionId { get; set; }

    }
}
