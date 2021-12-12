using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class ProductReview
    {
        [Key]
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string CustomersId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; }

        public User Customers { get; set; }

    }
}
