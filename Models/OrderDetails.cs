using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class OrderDetails
    {
        [Key]
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public float ItemQuantity { get; set; }
        public float ItemSubTotal { get; set; } 
    }
}
