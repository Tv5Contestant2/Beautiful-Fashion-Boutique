using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class CustomersShippingAddress
    {
        [Key]
        public int Id { get; set; }
        public string CustomerUserId { get; set; }
        public string EmailAddress { get; set; }
        public string Block { get; set; }
        public string Lot { get; set; }
        public string City { get; set; }
        public string Barangay { get; set; }
        public string ContactNumber { get; set; }
    }
}
