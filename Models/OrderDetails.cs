using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class OrderDetails
    {
        [Key]
        public long Id { get; set; }

        public Guid TransactionId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        public Product Product { get; set; }
    }
}