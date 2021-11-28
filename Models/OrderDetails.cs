using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class OrderDetails
    {
        [Key]
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public Product Product { get; set; }
    }
}
