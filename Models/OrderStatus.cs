using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
    }
}
