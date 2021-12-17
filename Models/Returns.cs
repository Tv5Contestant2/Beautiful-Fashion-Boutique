using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Returns
    {
        [Key]
        public long Id { get; set; }

        public Guid OrderReference { get; set; }
        public long ProductId { get; set; }
        public long ChangeProductsId { get; set; }
        public DateTime ReturnDate { get; set; }

        public Product ChangeProducts { get; set; }
        public OrderStatus ReturnStatus { get; set; }
    }
}