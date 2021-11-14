using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        public string History { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
    }
}
