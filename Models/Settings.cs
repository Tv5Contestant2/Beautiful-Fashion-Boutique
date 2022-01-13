using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
