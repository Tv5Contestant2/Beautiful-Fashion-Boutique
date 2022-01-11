using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class OrderShippingInfo
    {
        [Key]
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public string CustomersId { get; set; }
        public string EmailAddress { get; set; }
        public string Block { get; set; }
        public string Lot { get; set; }
        public string City { get; set; }
        public string Barangay { get; set; }
        public string ContactNumber { get; set; }
        public string Instructions { get; set; }
    }
}
