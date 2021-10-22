using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string AlternateTitle { get; set; }
    }
}
