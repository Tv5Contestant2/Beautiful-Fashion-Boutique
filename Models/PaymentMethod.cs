using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
