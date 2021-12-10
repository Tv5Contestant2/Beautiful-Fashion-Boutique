using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Returns
    {
        [Key]
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string CustomersId { get; set; }

        public Product Product { get; set; }
    }
}
