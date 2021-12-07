using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string CustomersId { get; set; }
        public string Instructions { get; set; }

        public Customers Customers { get; set; }
    }
}
