using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public long ProductId { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemSubTotal { get; set; } 
        public string ModeOfPayment { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public Product Product { get; set; }
    }
}
