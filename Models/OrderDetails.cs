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
        public bool IsReturned { get; set; }
        public int QuantityReturned { get; set; }
        public string ReturnReason { get; set; }
        public int? ReturnStatusId { get; set; }

        public Product Product { get; set; }
        public Orders Orders { get; set; }
        public OrderStatus ReturnStatus { get; set; }
    }
}