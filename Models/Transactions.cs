using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public float TransactionTotalPrice { get; set; }
        public int EmployeeID { get; set; }
        public string TransactionStatus { get; set; }
        public int CustomerID { get; set; }
    }
}
